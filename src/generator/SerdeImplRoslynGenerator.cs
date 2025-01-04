
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StaticCs;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;

namespace Serde;

/// <summary>
/// Recognizes the [GenerateSerialize] attribute on a type to generate an implementation
/// of Serde.ISerialize. The implementation generally looks like a call to SerializeType,
/// then successive calls to SerializeField.
/// </summary>
/// <example>
/// For a type like,
///
/// <code>
/// [GenerateSerialize]
/// partial struct Rgb { public byte Red; public byte Green; public byte Blue; }
/// </code>
///
/// The generated code would be,
///
/// <code>
/// using Serde;
///
/// partial struct Rgb : Serde.ISerialize
/// {
///     void Serde.ISerialize.Serialize&lt;TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary&gt;TSerializer serializer)
///     {
///         var type = serializer.SerializeType("Rgb", 3);
///         type.SerializeField("Red", new ByteWrap(Red));
///         type.SerializeField("Green", new ByteWrap(Green));
///         type.SerializeField("Blue", new ByteWrap(Blue));
///         type.End();
///     }
/// }
/// </code>
/// </example>
[Generator]
public class SerdeImplRoslynGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        static GenerationOutput GenerateForCtx(
            SerdeUsage usage,
            GeneratorAttributeSyntaxContext attrCtx,
            CancellationToken cancelToken)
        {
            var generationContext = new GeneratorExecutionContext(attrCtx);
            RunGeneration(usage, attrCtx, generationContext, cancelToken);
            return generationContext.GetOutput();

            static void RunGeneration(
                SerdeUsage usage,
                GeneratorAttributeSyntaxContext attrCtx,
                GeneratorExecutionContext generationContext,
                CancellationToken cancelToken)
            {
                var typeDecl = (BaseTypeDeclarationSyntax)attrCtx.TargetNode;
                var model = attrCtx.SemanticModel;
                var typeSymbol = model.GetDeclaredSymbol(typeDecl);
                if (typeSymbol is null)
                {
                    return;
                }
                // Assume that all symbol declarations are non-null, even if in an unnanotated
                // context
                typeSymbol = (INamedTypeSymbol)typeSymbol
                    .WithNullableAnnotation(NullableAnnotation.NotAnnotated);

                static bool PrivateOrCopyCtor(IMethodSymbol ctor)
                {
                    if (ctor.DeclaredAccessibility == Accessibility.Private)
                    {
                        return true;
                    }
                    if (ctor.Parameters is [{ Type: { } paramType }] && paramType.Equals(ctor.ContainingType, SymbolEqualityComparer.Default))
                    {
                        return true;
                    }
                    return false;
                }

                if (typeSymbol.IsAbstract &&
                    (!typeSymbol.IsRecord
                     || !typeSymbol.InstanceConstructors.All(c => PrivateOrCopyCtor(c))
                     || SymbolUtilities.GetDUTypeMembers(typeSymbol).Length == 0))
                {
                    generationContext.ReportDiagnostic(CreateDiagnostic(
                        DiagId.ERR_CantImplementAbstract,
                        typeDecl.Identifier.GetLocation(),
                        typeSymbol));
                    return;
                }

                var attributeData = attrCtx.Attributes.Single();

                INamedTypeSymbol receiverType = typeSymbol;
                var proxiedOpt = TryGetProxiedType(attributeData, model, typeDecl, typeSymbol);
                // If the Through property is set, then we are implementing a wrapper type
                if (proxiedOpt is { Item1: null })
                {
                    // Error already reported
                    return;
                }
                else if (proxiedOpt is { Item1: { } proxiedType })
                {
                    receiverType = proxiedType;

                    if (receiverType.SpecialType != SpecialType.None)
                    {
                        generationContext.ReportDiagnostic(CreateDiagnostic(
                            DiagId.ERR_CantWrapSpecialType,
                            attributeData.ApplicationSyntaxReference!.GetSyntax().GetLocation(),
                            receiverType));
                        return;
                    }
                }
                else if (!typeDecl.IsKind(SyntaxKind.EnumDeclaration))
                {
                    if (!typeDecl.Modifiers.Any(tok => tok.IsKind(SyntaxKind.PartialKeyword)))
                    {
                        // Type must be partial
                        generationContext.ReportDiagnostic(CreateDiagnostic(
                            DiagId.ERR_TypeNotPartial,
                            typeDecl.Identifier.GetLocation(),
                            typeDecl.Identifier.ValueText));
                        return;
                    }
                }

                if (typeDecl.IsKind(SyntaxKind.EnumDeclaration))
                {
                    Proxies.GenerateEnumProxy(
                        typeDecl,
                        attrCtx.SemanticModel,
                        generationContext);
                }

                var inProgress = ImmutableList.Create<(ITypeSymbol Receiver, ITypeSymbol Containing)>(
                    (receiverType.WithNullableAnnotation(NullableAnnotation.Annotated), typeSymbol));

                GenerateInfoAndSerdeImpls(usage, generationContext, typeDecl, receiverType, inProgress);
            }
        }

        var generateSerdeTypes = context.SyntaxProvider.ForAttributeWithMetadataName(
            WellKnownAttribute.GenerateSerde.GetFqn(),
            (_, _) => true,
            (attrCtx, cancelToken) => GenerateForCtx(SerdeUsage.Both, attrCtx, cancelToken));

        var generateSerializeTypes = context.SyntaxProvider.ForAttributeWithMetadataName(
            WellKnownAttribute.GenerateSerialize.GetFqn(),
            (_, _) => true,
            (attrCtx, cancelToken) => GenerateForCtx(SerdeUsage.Serialize, attrCtx, cancelToken));

        var generateDeserializeTypes = context.SyntaxProvider.ForAttributeWithMetadataName(
            WellKnownAttribute.GenerateDeserialize.GetFqn(),
            (_, _) => true,
            (attrCtx, cancelToken) => GenerateForCtx(SerdeUsage.Deserialize, attrCtx, cancelToken));

        // Combine GenerateSerialize and GenerateDeserialize in case they both need to generate an identical
        // helper type.
        var combined = generateSerializeTypes.Collect()
            .Combine(generateDeserializeTypes.Collect())
            .Select(static (values, _) =>
            {
                var (serialize, deserialize) = values;
                return new GenerationOutput(
                    serialize.SelectMany(static o => o.Diagnostics).Concat(deserialize.SelectMany(static o => o.Diagnostics)),
                    serialize.SelectMany(static o => o.Sources).Concat(deserialize.SelectMany(static o => o.Sources)));
            });

        var provideOutput = static (SourceProductionContext ctx, GenerationOutput output) =>
        {
            foreach (var d in output.Diagnostics)
            {
                ctx.ReportDiagnostic(d);
            }
            foreach (var (fileName, content) in output.Sources)
            {
                ctx.AddSource(fileName, content.ToSourceText());
            }
        };

        context.RegisterSourceOutput(generateSerdeTypes, provideOutput);
        context.RegisterSourceOutput(combined, provideOutput);
    }


    internal static void GenerateInfoAndSerdeImpls(
        SerdeUsage usage,
        GeneratorExecutionContext generationContext,
        BaseTypeDeclarationSyntax typeDecl,
        INamedTypeSymbol receiverType,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        SerdeInfoGenerator.GenerateSerdeInfo(
            typeDecl,
            receiverType,
            generationContext,
            usage,
            inProgress);

        if (usage.HasFlag(SerdeUsage.Serialize))
        {
            GenerateImpl(
                SerdeUsage.Serialize,
                new TypeDeclContext(typeDecl),
                receiverType,
                generationContext,
                inProgress);
        }

        if (usage.HasFlag(SerdeUsage.Deserialize))
        {
            GenerateImpl(
                SerdeUsage.Deserialize,
                new TypeDeclContext(typeDecl),
                receiverType,
                generationContext,
                inProgress);
        }
    }

    /// <summary>
    /// If the type has a `Through` property then we are generating a proxy type and need to find
    /// the proxied type.
    /// </summary>
    /// <returns>
    /// Returns null if there is no `Through` property. Returns ValueTuple(null) if there is an
    /// error. Returns ValueTuple(ITypeSymbol) if there is a valid proxied type.
    /// </returns>
    private static ValueTuple<INamedTypeSymbol?>? TryGetProxiedType(
        AttributeData attributeData,
        SemanticModel model,
        BaseTypeDeclarationSyntax typeDecl,
        ITypeSymbol attributedSymbol)
    {
        foreach (var namedArg in attributeData.NamedArguments)
        {
            switch (namedArg)
            {
                case { Key: nameof(GenerateSerialize.ForType),
                       Value: { Kind: TypedConstantKind.Type, Value: ITypeSymbol typeSymbol } }:
                    if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                    {
                        // TODO: Error about bad type
                        return new(null);
                    }
                    return new(namedTypeSymbol);
            }
        }
        return null;
    }

    internal static void GenerateImpl(
        SerdeUsage usage,
        TypeDeclContext typeDeclContext,
        ITypeSymbol receiverType,
        GeneratorExecutionContext context,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        var typeName = typeDeclContext.Name;

        // Generate statements for the implementation
        var (implMembers, baseList) = usage switch
        {
            SerdeUsage.Serialize => SerializeImplGen.GenSerialize(typeDeclContext, context, receiverType, inProgress),
            SerdeUsage.Deserialize => DeserializeImplGen.GenDeserialize(typeDeclContext, context, receiverType, inProgress),
            _ => throw ExceptionUtilities.Unreachable
        };

        var typeKind = typeDeclContext.Kind;
        var newType = new SourceBuilder("""

            #nullable enable

            using System;
            using Serde;

            """
        );

        if (typeKind == SyntaxKind.EnumDeclaration)
        {
            var proxyName = Proxies.GetProxyName(typeName);
            var siblingType = typeDeclContext.MakeSiblingType(new SourceBuilder(
                $$"""
                sealed partial class {{proxyName}} {{baseList}}
                {
                    {{implMembers}}
                }
                """
            ));
            newType.Append(siblingType);
            typeName = proxyName;
        }
        else
        {
            var suffix = usage == SerdeUsage.Serialize ? "Serialize" : "Deserialize";
            var proxyName = typeName + suffix + "Proxy";

            implMembers.AppendLine(
                $"public static readonly {proxyName} Instance = new();"
            );
            implMembers.AppendLine(
                $$"""
                private {{proxyName}}() { }
                """
            );

            var interfaceName = usage.GetProxyInterfaceName();
            var proxyType = new SourceBuilder($$"""
            sealed partial class {{proxyName}} {{baseList}}
            {
                {{implMembers}}
            }
            """);

            var members = new SourceBuilder($$"""
            static {{interfaceName}}<{{receiverType.ToDisplayString()}}> {{interfaceName}}Provider<{{receiverType.ToDisplayString()}}>.{{suffix}}Instance
                => {{proxyName}}.Instance;

            {{proxyType}}
            """);
            var providerString = $"Serde.{interfaceName}Provider<{receiverType.ToDisplayString()}>";
            typeDeclContext.AppendPartialDecl(newType, providerString, members);
        }
        string fullTypeName = string.Join(".", typeDeclContext.NamespaceNames
            .Concat(typeDeclContext.ParentTypeInfo.Select(x => x.Name))
            .Concat(new[] { typeName }));

        var srcName = fullTypeName + "." + usage.GetProxyInterfaceName();

        context.AddSource(srcName, newType);
    }

    internal static (string FileName, SourceBuilder Decl) MakePartialDecl(
        TypeDeclContext typeDeclContext,
        BaseListSyntax? baseList,
        SourceBuilder implMembers,
        string fileNameSuffix)
    {
        string typeName = typeDeclContext.Name;
        var typeKind = typeDeclContext.Kind;
        string declKeywords;
        (typeName, declKeywords) = typeKind == SyntaxKind.EnumDeclaration
            ? (Proxies.GetProxyName(typeName), "class")
            : (typeName, TypeDeclContext.TypeKindToString(typeKind));
        var newType = new SourceBuilder($$"""
partial {{declKeywords}} {{typeName}}{{typeDeclContext.TypeParameterList}} : Serde.ISerdeInfoProvider
{
    {{implMembers}}
}
""");
        string fullTypeName = string.Join(".", typeDeclContext.NamespaceNames
            .Concat(typeDeclContext.ParentTypeInfo.Select(x => x.Name))
            .Concat(new[] { typeName }));

        var srcName = fullTypeName + "." + fileNameSuffix;

        newType = typeDeclContext.MakeSiblingType(newType);
        newType = new($"""

        #nullable enable
        {newType}
        """);

        return (srcName, newType);
    }

    /// <summary>
    /// Check to see if the <paramref name="targetType"/> implements ISerialize{<paramref
    /// name="argType"/>} or IDeserialize{<paramref name="argType"/>}, depending on the WrapUsage.
    /// </summary>
    internal static bool ImplementsSerde(ITypeSymbol targetType, ITypeSymbol argType, GeneratorExecutionContext context, SerdeUsage usage)
    {
        // Nullable types are not considered as implementing the Serde interfaces -- they use wrappers to map to the underlying
        if (targetType.NullableAnnotation == NullableAnnotation.Annotated ||
            targetType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
        {
            return false;
        }

        // Check if the type either has the GenerateSerialize attribute, or directly implements ISerialize
        // (If the type has the GenerateSerialize attribute then the generator will implement the interface)
        if (targetType.TypeKind is not TypeKind.Enum && HasGenerateAttribute(targetType, usage))
        {
            return true;
        }

        var mdName = usage switch {
            SerdeUsage.Serialize => "Serde.ISerializeProvider`1",
            SerdeUsage.Deserialize => "Serde.IDeserializeProvider`1",
            _ => throw new ArgumentException("Invalid SerdeUsage", nameof(usage))
        };
        var serdeSymbol = context.Compilation.GetTypeByMetadataName(mdName)?.Construct(argType);

        if (serdeSymbol is not null && targetType.AllInterfaces.Contains(serdeSymbol, SymbolEqualityComparer.Default)
            || (targetType is ITypeParameterSymbol param && param.ConstraintTypes.Contains(serdeSymbol, SymbolEqualityComparer.Default)))
        {
            return true;
        }
        return false;
    }

    internal static bool HasGenerateAttribute(ITypeSymbol memberType, SerdeUsage usage)
    {
        var attributes = memberType.GetAttributes();
        foreach (var attr in attributes)
        {
            var attrClass = attr.AttributeClass;
            if (attrClass is null)
            {
                continue;
            }
            if (WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.GenerateSerde))
            {
                return true;
            }
            if (usage == SerdeUsage.Serialize &&
                WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.GenerateSerialize))
            {
                return true;
            }
            if (usage == SerdeUsage.Deserialize &&
                WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.GenerateDeserialize))
            {
                return true;
            }
        }
        return false;
    }

    internal static string GetSerdeName(INamedTypeSymbol type)
    {
        var options = SymbolUtilities.GetTypeOptions(type);
        if (options.Rename is { } rename)
        {
            return rename;
        }
        return type.Name;
    }
}

[Flags]
[Closed]
internal enum SerdeUsage : byte
{
    Serialize = 0b01,
    Deserialize = 0b10,

    Both = Serialize | Deserialize,
}