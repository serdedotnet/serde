
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Serde;

internal static class SerdeInfoGenerator
{
    /// <summary>
    /// Generate the TypeInfo for the given type declaration.
    /// Looks like:
    /// <code>
    /// partial {typeKind} {typeName}
    /// {
    ///     internal static ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
    ///         {typeName},
    ///         typeof({typeName}).GetCustomAttributesData(),
    ///         [
    ///             new("{{fieldName}}", SerdeInfoProvider.GetInfo&lt;{wrapperName}&gt;())
    ///             {
    ///                 MemberInfo = typeof({typeName}).GetField("{fieldName}"),
    ///             },
    ///             ...
    ///         ]);
    /// }
    /// </code>
    /// </summary>
    public static void GenerateSerdeInfo(
        TypeDeclContext typeDeclContext,
        INamedTypeSymbol receiverType,
        INamedTypeSymbol? foreignType,
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress
    )
    {
        if (receiverType.IsAbstract)
        {
            GenerateUnionInfoAndSerdeImpls(typeDeclContext.TypeDecl, receiverType, context, usage, inProgress);
            return;
        }

        // For custom types we only a single SerdeUsage, either Serialize or Deserialize.
        // For Both we'll normalize to Serialize.
        if (usage == SerdeUsage.Both)
        {
            usage = SerdeUsage.Serialize;
        }

        bool isEnum = receiverType.TypeKind == TypeKind.Enum;
        string makeFuncSuffix;
        string fieldArrayType;
        if (isEnum)
        {
            makeFuncSuffix = "Enum";
            fieldArrayType = "(string, System.Reflection.MemberInfo?)[]";
        }
        else
        {
            makeFuncSuffix = "Custom";
            fieldArrayType = "global::Serde.SerdeInfo.FieldInfo[]";
        }

        // reflectionType is used for typeof() in reflection calls.
        var reflectionType = receiverType;
        var reflectionTypeString = reflectionType.ToDisplayString();
        if (reflectionTypeString.IndexOf('<') is var index && index != -1)
        {
            reflectionTypeString = reflectionTypeString[..index];
            reflectionTypeString = reflectionTypeString + "<" + new string(',', reflectionType is INamedTypeSymbol nts ? nts.TypeParameters.Length - 1 : 0) + ">";
        }

        var classScopeProxyMap = ProxyMap.FromSymbol(receiverType);

        // Materialize the (member, wrapper) pairs once so the emitted field array stays aligned, and
        // so GetWrapperName's diagnostics fire only once per member.
        var dataMembers = SymbolUtilities.GetDataMembers(receiverType, SerdeUsage.Both, context);
        var emittedMembers = new List<(DataMemberSymbol Member, string? Wrapper)>();
        foreach (var m in dataMembers)
        {
            if (isEnum)
            {
                emittedMembers.Add((m, null));
                continue;
            }

            var proxyContext = ProxyContext.Create(classScopeProxyMap, ProxyMap.FromSymbol(m.Symbol));
            var wrapperName = GetWrapperName(m, context, usage, inProgress, proxyContext);
            if (wrapperName is null)
            {
                // This is an error, but it should have already been produced by the serialization
                // or deserialization generator.
                continue;
            }
            emittedMembers.Add((m, wrapperName));
        }

        // When every member has an explicit ordinal, emit the ordinals (in physical/field-array
        // order). Their presence is what makes the generated ISerdeInfo report
        // HasExplicitFieldOrdinals == true, so the ordinals are emitted even when they happen to
        // equal the default 0..n sequence: that presence distinguishes "explicitly assigned" (a
        // stable identity) from "incidental physical position". The partial case (some but not all)
        // is a reported error and is skipped here to avoid emitting bogus data.
        var allExplicit = !isEnum
            && emittedMembers.Count > 0
            && emittedMembers.All(e => e.Member.Ordinal is int);

        var memberEntries = new List<string>(emittedMembers.Count);
        foreach (var (m, wrapper) in emittedMembers)
        {
            memberEntries.Add(GetMemberEntry(m, wrapper, allExplicit));
        }

        // Type identity: the SerdeInfo Name is the conceptual type being
        // (de)serialized. For a non-empty conversion proxy that is the foreign
        // type, not the proxy (which is an implementation detail). Type-level
        // attributes and member reflection still come from the proxy, since
        // that is where serde configuration and the mirrored members live.
        var typeString = receiverType.ToDisplayString();
        if (typeString.IndexOf('<') is var idx && idx != -1)
        {
            typeString = typeString[..idx];
            typeString = typeString + "<" + new string(',', receiverType.TypeParameters.Length - 1) + ">";
        }
        var identityName = (foreignType ?? receiverType).Name;
        List<string> makeArgs = [ $"\"{identityName}\"", $"typeof({typeString}).GetCustomAttributesData()" ];

        if (isEnum)
        {
            var proxyContext = ProxyContext.Create(classScopeProxyMap, ProxyMap.Empty);

            string underlyingInfo;
            if (Proxies.TryGetImplicitWrapper(receiverType.EnumUnderlyingType!, context, usage, inProgress, proxyContext) is { Proxy: { } wrap })
            {
                underlyingInfo = wrap.ToString();
            }
            else
            {
                // This should never happen. Produce a bogus string
                underlyingInfo = "<underlying info not found, this is a bug>";
            }
            string name = usage == SerdeUsage.Serialize ? "Serialize" : "Deserialize";
            makeArgs.Add($"global::Serde.SerdeInfoProvider.Get{name}Info<{receiverType.EnumUnderlyingType}, {underlyingInfo}>()");
        }

        // Build the field array argument with explicit, self-consistent indentation so each
        // FieldInfo block nests cleanly under the array initializer.
        var arrayBuilder = new SourceBuilder();
        arrayBuilder.AppendLine($"new {fieldArrayType} {{");
        arrayBuilder.Indent();
        for (int i = 0; i < memberEntries.Count; i++)
        {
            var trailing = i < memberEntries.Count - 1 ? "," : "";
            arrayBuilder.AppendLine(memberEntries[i] + trailing);
        }
        arrayBuilder.Dedent();
        arrayBuilder.Append("}");
        makeArgs.Add(arrayBuilder.ToString());

        // Assemble the MakeXxx(...) call. Each argument is appended on its own line at a single
        // indent level; multi-line arguments (the field array) are indented uniformly so the type
        // name, attributes, and field array all line up.
        var openLine = isEnum
            ? $"global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo {{ get; }} = Serde.SerdeInfo.Make{makeFuncSuffix}("
            : $"private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.Make{makeFuncSuffix}(";

        var body = new SourceBuilder();
        body.AppendLine(openLine);
        body.Indent();
        for (int i = 0; i < makeArgs.Count; i++)
        {
            var trailing = i < makeArgs.Count - 1 ? "," : "";
            body.AppendLine(makeArgs[i] + trailing);
        }
        body.Dedent();
        body.Append(");");
        var (fileName, newType) = SerdeImplRoslynGenerator.MakePartialDecl(
            typeDeclContext,
            baseList: isEnum ? " : global::Serde.ISerdeInfoProvider" : null,
            body,
            "ISerdeInfoProvider");

        context.AddSource(fileName, newType);

        // Returns a string that represents a single member in the custom/enum SerdeInfo member list.
        // For enums this is the (string, MemberInfo?) tuple consumed by MakeEnum; for custom types
        // this is a Serde.SerdeInfo.FieldInfo object initializer consumed by MakeCustom.
        string GetMemberEntry(DataMemberSymbol m, string? wrapperName, bool emitOrdinal)
        {
            var getAccessor = m.Symbol.Kind == SymbolKind.Field ? "GetField" : "GetProperty";
            // Member reflection points at the member source type (proxy when non-empty).
            var memberInfoExpr = $"typeof({reflectionTypeString}).{getAccessor}(\"{m.Name}\")";

            if (isEnum)
            {
                return $"(\"{m.GetFormattedName()}\", {memberInfoExpr})";
            }

            string name = usage == SerdeUsage.Serialize ? "Serialize" : "Deserialize";
            var infoExpr = $"global::Serde.SerdeInfoProvider.Get{name}Info<{m.Type.ToDisplayString()}, {wrapperName}>()";

            // MemberInfo only exists to lazily supply the field's custom attributes. If the member
            // has no attributes, skip it entirely so the runtime never does the reflection lookup.
            var emitMemberInfo = !m.Attributes.IsEmpty;

            var sb = new StringBuilder();
            sb.Append($"new(\"{m.GetFormattedName()}\", {infoExpr})");
            if (emitMemberInfo || emitOrdinal)
            {
                sb.Append(Utilities.NewLine).Append("{").Append(Utilities.NewLine);
                if (emitMemberInfo)
                {
                    sb.Append($"    MemberInfo = {memberInfoExpr},").Append(Utilities.NewLine);
                }
                if (emitOrdinal)
                {
                    sb.Append($"    Ordinal = {(int)m.Ordinal!},").Append(Utilities.NewLine);
                }
                sb.Append("}");
            }
            return sb.ToString();
        }
    }

    private static void GenerateUnionInfoAndSerdeImpls(
        BaseTypeDeclarationSyntax typeDecl,
        INamedTypeSymbol receiverType,
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        var typeMembers = SymbolUtilities.GetDUTypeMembers(receiverType);
        var originalCtx = new TypeDeclContext(typeDecl);
        foreach (var m in typeMembers)
        {
            var proxyName = GetUnionProxyName(m);
            string typeName = originalCtx.Name;
            var typeKind = originalCtx.Kind;
            string declKeywords;
            (typeName, declKeywords) = typeKind == SyntaxKind.EnumDeclaration
                ? (Proxies.GetProxyName(typeName), "class")
                : (typeName, TypeDeclContext.TypeKindToString(typeKind));
            var nestedType = originalCtx.MakeSiblingType(new SourceBuilder($$"""
partial {{declKeywords}} {{typeName}}{{originalCtx.TypeParameterList}}
{
    private sealed partial class {{proxyName}} {}
}
"""));
            var newCtx = TypeDeclContext.FromFile(nestedType.ToString(), proxyName);
            SerdeImplRoslynGenerator.GenerateInfoAndSerdeImpls(usage, context, newCtx, m, foreignType: null, inProgress.Add((m, receiverType)));
            var serdeObjFqn = $"{newCtx.GetFqn()}.{usage.GetSerdeObjName()}";
            SerdeImplRoslynGenerator.GenerateProviderImpls(usage, context, serdeObjFqn, newCtx, m, foreignType: null);
        }

        var bodies = new SourceBuilder($$"""
private static global::Serde.ISerdeInfo s_serdeInfo { get; } = Serde.SerdeInfo.MakeUnion(
    "{{SerdeImplRoslynGenerator.GetSerdeName(receiverType)}}",
    typeof({{receiverType.ToDisplayString()}}).GetCustomAttributesData(),
    System.Collections.Immutable.ImmutableArray.Create<global::Serde.ISerdeInfo>(
        {{GetMembersInfos()}}
    )
);

{{GetProxyDefs()}}
""");

        var typeDeclContext = new TypeDeclContext(typeDecl);
        var (fileName, newType) = SerdeImplRoslynGenerator.MakePartialDecl(
            typeDeclContext,
            baseList: null,
            bodies,
            "ISerdeInfoProvider");
        context.AddSource(fileName, newType);

        return;

        string GetMembersInfos()
        {
            string infoName = usage.HasFlag(SerdeUsage.Serialize) ? "Serialize" : "Deserialize";
            return string.Join("," + Utilities.NewLine,
                typeMembers.Select(m => $"global::Serde.SerdeInfoProvider.Get{infoName}Info<{m.ToDisplayString()}, {GetUnionProxyName(m)}>()"));
        }

        string GetProxyDefs()
        {
            string attrString = usage switch
            {
                SerdeUsage.Both => "Serde",
                SerdeUsage.Serialize => "Serialize",
                SerdeUsage.Deserialize => "Deserialize",
            };
            var sb = new StringBuilder();
            foreach (var m in typeMembers)
            {
                sb.AppendLine($"[global::Serde.Generate{attrString}]");
                sb.AppendLine($"private sealed partial class {GetUnionProxyName(m)} {{}}");
            }
            return sb.ToString();
        }
    }

    internal static string GetUnionProxyName(INamedTypeSymbol member) => $"_m_{member.Name}Proxy";

    private static string? GetWrapperName(
        DataMemberSymbol m,
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress,
        ProxyContext proxyContext)
    {
        return Proxies.TryGetProxyString(m.Symbol, m.Type, context, usage, inProgress, proxyContext);
    }
}