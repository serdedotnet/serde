
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

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
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
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

        var typeString = receiverType.ToDisplayString();
        if (typeString.IndexOf('<') is var index && index != -1)
        {
            typeString = typeString[..index];
            typeString = typeString + "<" + new string(',', receiverType.TypeParameters.Length - 1) + ">";
        }

        var membersString = string.Join("," + Utilities.NewLine,
            SymbolUtilities.GetDataMembers(receiverType, SerdeUsage.Both).SelectNotNull(GetMemberEntry));

        List<string> makeArgs = [ $"\"{receiverType.Name}\"", $"typeof({typeString}).GetCustomAttributesData()" ];

        if (isEnum)
        {
            string underlyingInfo;
            if (Proxies.TryGetImplicitWrapper(receiverType.EnumUnderlyingType!, context, usage, inProgress) is { Proxy: { } wrap })
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
                string? wrapperName = GetWrapperName(m, context, usage, inProgress);
                if (wrapperName is null)
                {
                    // This is an error, but it should have already been produced by the serialization
                    // or deserialization generator
                    return null;
                }
                string name = usage == SerdeUsage.Serialize ? "Serialize" : "Deserialize";
                elements.Add($"global::Serde.SerdeInfoProvider.Get{name}Info<{m.Type.ToDisplayString()}, {wrapperName}>()");
            }

            var getAccessor = m.Symbol.Kind == SymbolKind.Field ? "GetField" : "GetProperty";
            elements.Add($"typeof({typeString}).{getAccessor}(\"{m.Name}\")");

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
            SerdeImplRoslynGenerator.GenerateInfoAndSerdeImpls(usage, context, newCtx, m, inProgress.Add((m, receiverType)));
            var serdeObjFqn = $"{newCtx.GetFqn()}.{usage.GetSerdeObjName()}";
            SerdeImplRoslynGenerator.GenerateProviderImpls(usage, context, serdeObjFqn, newCtx, m);
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
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        return Proxies.TryGetProxyString(m.Symbol, m.Type, context, usage, inProgress);
    }
}