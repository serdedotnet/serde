
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
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
    ///             ("{{fieldName}}", SerdeInfoProvider.GetInfo&lt;{wrapperName}&gt;typeof({typeName}).GetField("{fieldName}")!),
    ///             ...
    ///         ]);
    /// }
    /// </code>
    /// </summary>
    public static void GenerateSerdeInfo(
        BaseTypeDeclarationSyntax typeDecl,
        INamedTypeSymbol receiverType,
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<ITypeSymbol> inProgress)
    {
        bool isEnum;
        string makeFuncSuffix;
        string fieldArrayType;
        if (receiverType.TypeKind == TypeKind.Enum)
        {
            isEnum = true;
            makeFuncSuffix = "Enum";
            fieldArrayType = "(string, System.Reflection.MemberInfo)[]";
        }
        else
        {
            isEnum = false;
            makeFuncSuffix = "Custom";
            fieldArrayType = "(string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[]";
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
            if (Wrappers.TryGetImplicitWrapper(receiverType.EnumUnderlyingType!, context, usage, inProgress) is { Wrapper: { } wrap })
            {
                underlyingInfo = wrap.ToString();
            }
            else
            {
                // This should never happen. Produce a bogus string
                underlyingInfo = "<underlying info not found, this is a bug>";
            }
            makeArgs.Add($"global::Serde.SerdeInfoProvider.GetInfo<{underlyingInfo}>()");
        }

        makeArgs.Add($$"""
new {{fieldArrayType}} {
{{membersString}}
    }
""");

        var argsString = string.Join("," + Utilities.NewLine + "        ", makeArgs);

        var body = $$"""
static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Make{{makeFuncSuffix}}(
        {{argsString}});
""";
        var typeDeclContext = new TypeDeclContext(typeDecl);
        var (fileName, newType) = SerdeImplRoslynGenerator.MakePartialDecl(
            typeDeclContext,
            baseList: null,
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
                elements.Add($"global::Serde.SerdeInfoProvider.GetInfo<{wrapperName}>()");
            }

            var getAccessor = m.Symbol.Kind == SymbolKind.Field ? "GetField" : "GetProperty";
            elements.Add($"typeof({typeString}).{getAccessor}(\"{m.Name}\")!");

            return $"""({string.Join(", ", elements)})""";
        }
    }

    private static string? GetWrapperName(
        DataMemberSymbol m,
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<ITypeSymbol> inProgress)
    {
        string? wrapperName;
        if (Wrappers.TryGetExplicitWrapper(m, context, usage, inProgress) is { } explicitWrap)
        {
            wrapperName = explicitWrap.ToString();
        }
        else if (SerdeImplRoslynGenerator.ImplementsSerde(m.Type, m.Type, context, usage))
        {
            wrapperName = m.Type.WithNullableAnnotation(m.NullableAnnotation).ToDisplayString();
        }
        else if (Wrappers.TryGetImplicitWrapper(m.Type, context, usage, inProgress) is { Wrapper: { } wrap })
        {
            wrapperName = wrap.ToString();
        }
        else
        {
            wrapperName = null;
        }
        return wrapperName;
    }
}