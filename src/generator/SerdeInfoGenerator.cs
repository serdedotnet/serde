
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
    ///             ("{{fieldName}}", SerdeInfoProvider.GetInfo&lt;{wrapperName}&gt;typeof({typeName}).GetField("{fieldName}")),
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
            fieldArrayType = "(string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[]";
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

        // Materialize the (member, entry) pairs once so the emitted field array and the parallel
        // ordinal array stay aligned, and so GetMemberEntry's diagnostics fire only once per member.
        var dataMembers = SymbolUtilities.GetDataMembers(receiverType, SerdeUsage.Both, context);
        var emittedMembers = new List<DataMemberSymbol>();
        var memberEntries = new List<string>();
        foreach (var m in dataMembers)
        {
            if (GetMemberEntry(m) is { } entry)
            {
                emittedMembers.Add(m);
                memberEntries.Add(entry);
            }
        }

        var membersString = string.Join("," + Utilities.NewLine, memberEntries);

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

        makeArgs.Add(new SourceBuilder($$"""
new {{fieldArrayType}} {
    {{membersString}}
}
""").ToString());

        // When every member has an explicit ordinal, emit the ordinals (in physical/field-array
        // order). Their presence is what makes the generated ISerdeInfo report
        // HasExplicitFieldOrdinals == true, so the array is emitted even when the ordinals happen to
        // equal the default 0..n sequence: that presence distinguishes "explicitly assigned" (a
        // stable identity) from "incidental physical position". The partial case (some but not all)
        // is a reported error and is skipped here to avoid emitting bogus data.
        if (!isEnum && emittedMembers.Count > 0)
        {
            var ordinals = new List<int>(emittedMembers.Count);
            var allExplicit = true;
            foreach (var m in emittedMembers)
            {
                if (m.Ordinal is not int ord)
                {
                    allExplicit = false;
                    break;
                }
                ordinals.Add(ord);
            }
            if (allExplicit)
            {
                makeArgs.Add($"new int[] {{ {string.Join(", ", ordinals)} }}");
            }
        }

        var argsString = string.Join("," + Utilities.NewLine, makeArgs);

        var body = new SourceBuilder(
            isEnum ? $$"""
global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Make{{makeFuncSuffix}}(
    {{argsString}}
);
"""
            : $$"""
private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.Make{{makeFuncSuffix}}(
    {{argsString}}
);
""");
        var (fileName, newType) = SerdeImplRoslynGenerator.MakePartialDecl(
            typeDeclContext,
            baseList: isEnum ? " : global::Serde.ISerdeInfoProvider" : null,
            body,
            "ISerdeInfoProvider");

        context.AddSource(fileName, newType);

        // Returns a string that represents a single member in the custom SerdeInfo member list.
        string? GetMemberEntry(DataMemberSymbol m)
        {
            List<string> elements = [ $"\"{m.GetFormattedName()}\"" ];

            if (!isEnum)
            {
                var proxyContext = ProxyContext.Create(classScopeProxyMap, ProxyMap.FromSymbol(m.Symbol));

                string? wrapperName = GetWrapperName(m, context, usage, inProgress, proxyContext);
                if (wrapperName is null)
                {
                    // This is an error, but it should have already been produced by the serialization
                    // or deserialization generator
                    return null;
                }
                string name = usage == SerdeUsage.Serialize ? "Serialize" : "Deserialize";
                elements.Add($"global::Serde.SerdeInfoProvider.Get{name}Info<{m.Type.ToDisplayString()}, {wrapperName}>()");
            }

            // Member reflection points at the member source type (proxy when non-empty)
            var getAccessor = m.Symbol.Kind == SymbolKind.Field ? "GetField" : "GetProperty";
            elements.Add($"typeof({reflectionTypeString}).{getAccessor}(\"{m.Name}\")");

            return $"""({string.Join(", ", elements)})""";
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