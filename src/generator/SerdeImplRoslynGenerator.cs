
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
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
/// then successive calls to WriteValue.
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
///         type.WriteValue("Red", new ByteWrap(Red));
///         type.WriteValue("Green", new ByteWrap(Green));
///         type.WriteValue("Blue", new ByteWrap(Blue));
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
                ctx.AddSource(fileName + ".g", content.ToSourceText());
            }
        };

        context.RegisterSourceOutput(generateSerdeTypes, provideOutput);
        context.RegisterSourceOutput(combined, provideOutput);

        return;

        static GenerationOutput GenerateForCtx(
            SerdeUsage usage,
            GeneratorAttributeSyntaxContext attrCtx,
            CancellationToken cancelToken)
        {
            var generationContext = new GeneratorExecutionContext(attrCtx);
            RunGeneration(usage, attrCtx, generationContext);
            return generationContext.GetOutput();
        }
    }

    private static void RunGeneration(
        SerdeUsage usage,
        GeneratorAttributeSyntaxContext attrCtx,
        GeneratorExecutionContext generationContext)
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
        var proxiedOpt = TryGetProxiedType(attributeData);
        bool isEnum = typeDecl.IsKind(SyntaxKind.EnumDeclaration);
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
        else if (!isEnum)
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

        var typeDeclContext = new TypeDeclContext(typeDecl);
        if (isEnum)
        {
            typeDeclContext = Proxies.GenerateEnumProxy(typeDeclContext, generationContext);
        }

        var inProgress = ImmutableList.Create<(ITypeSymbol Receiver, ITypeSymbol Containing)>(
            (receiverType.WithNullableAnnotation(NullableAnnotation.Annotated), typeSymbol));

        string serdeObjString;
        if (TryGetSerdeObj(attributeData) is not { Item1: { } serdeObj })
        {
            GenerateInfoAndSerdeImpls(usage, generationContext, typeDeclContext, receiverType, inProgress);
            serdeObjString = isEnum
                ? typeDeclContext.GetFqn()
                : $"{typeDeclContext.GetFqn()}.{usage.GetSerdeObjName()}";
        }
        else
        {
            // If the user provided a custom serde object, we only need to attach it, meaning just
            // generate the provider implementations.
            serdeObjString = serdeObj.ToFqn();
        }

        GenerateProviderImpls(
            usage,
            generationContext,
            serdeObjString,
            typeDeclContext,
            receiverType);
    }

    internal static void GenerateProviderImpls(
        SerdeUsage usage,
        GeneratorExecutionContext generationContext,
        string serdeObjName,
        TypeDeclContext typeDeclContext,
        INamedTypeSymbol receiverType)
    {
        string fullTypeName = typeDeclContext.GetFqn(includeTypeParameters: false);

        string srcName;
        SourceBuilder content;
        content = GenProviderImplHelper(usage);
        srcName = $"{fullTypeName}.{usage.GetInterfaceName()}Provider";
        generationContext.AddSource(srcName, content);

        SourceBuilder GenProviderImplHelper(SerdeUsage usage)
            => GenProviderImpl(usage, serdeObjName, typeDeclContext, receiverType);
    }

    private static SourceBuilder GenProviderImpl(
        SerdeUsage usage,
        string serdeObjName,
        TypeDeclContext typeDeclContext,
        ITypeSymbol receiverType
    )
    {
        var receiverString = receiverType.ToDisplayString();
        var containerString = typeDeclContext.GetFqn();

        string baseList;
        SourceBuilder members;
        if (usage == SerdeUsage.Both)
        {
            baseList = $" : Serde.ISerdeProvider<{containerString}, {serdeObjName}, {receiverString}>";
            members = new SourceBuilder($$"""
            static {{serdeObjName}} global::Serde.ISerdeProvider<{{containerString}}, {{serdeObjName}}, {{receiverString}}>.Instance { get; }
                = new {{serdeObjName}}();
            """);
        }
        else
        {
            var interfaceName = $"{usage.GetInterfaceName()}Provider";
            baseList = $" : Serde.{interfaceName}<{receiverType.ToFqn()}>";
            members = new SourceBuilder($$"""
            static global::Serde.{{usage.GetInterfaceName()}}<{{receiverType.ToDisplayString()}}> global::Serde.{{interfaceName}}<{{receiverType.ToFqn()}}>.Instance { get; }
                = new {{serdeObjName}}();
            """);

        }

        var newType = new SourceBuilder();
        typeDeclContext.AppendPartialDecl(newType, baseList, members);
        return newType;
    }


    internal static void GenerateInfoAndSerdeImpls(
        SerdeUsage usage,
        GeneratorExecutionContext generationContext,
        TypeDeclContext typeDeclContext,
        INamedTypeSymbol receiverType,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        SerdeInfoGenerator.GenerateSerdeInfo(
            typeDeclContext,
            receiverType,
            generationContext,
            usage,
            inProgress);

        GenSerdeObjImpl(
            usage,
            typeDeclContext,
            receiverType,
            generationContext,
            inProgress);
    }

    /// <summary>
    /// If the type has a `Through` property then we are generating a proxy type and need to find
    /// the proxied type.
    /// </summary>
    /// <returns>
    /// Returns null if there is no `Through` property. Returns ValueTuple(null) if there is an
    /// error. Returns ValueTuple(ITypeSymbol) if there is a valid proxied type.
    /// </returns>
    private static ValueTuple<INamedTypeSymbol?>? TryGetProxiedType(AttributeData attributeData)
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

    /// <summary>
    /// Check if the generation attribute has a <see cref="GenerateSerde.With"/> property,
    /// which specifies a custom serde object to use for serialization or deserialization.
    /// </summary>
    private static ValueTuple<INamedTypeSymbol?>? TryGetSerdeObj(AttributeData attributeData)
    {
        foreach (var namedArg in attributeData.NamedArguments)
        {
            switch (namedArg)
            {
                case { Key: nameof(GenerateSerde.With),
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

    internal static void GenSerdeObjImpl(
        SerdeUsage usage,
        TypeDeclContext typeDeclContext,
        ITypeSymbol receiverType,
        GeneratorExecutionContext context,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        string fullTypeName = typeDeclContext.GetFqn(includeTypeParameters: false);
        var fullTypeString = typeDeclContext.GetFqn();

        // Generate statements for the implementation
        SourceBuilder implMembers;
        string? baseList;
        switch (usage)
        {
            case SerdeUsage.Serialize:
                implMembers = SerializeImplGen.GenSerialize(context, receiverType, inProgress);
                baseList = $" : Serde.ISerialize<{receiverType.ToDisplayString()}>";
                break;
            case SerdeUsage.Deserialize:
                implMembers = DeserializeImplGen.GenDeserialize(context, receiverType, inProgress);
                baseList = $" : Serde.IDeserialize<{receiverType.ToDisplayString()}>";
                break;
            case SerdeUsage.Both:
                implMembers = SerializeImplGen.GenSerialize(context, receiverType, inProgress);
                var deserializeMembers = DeserializeImplGen.GenDeserialize(context, receiverType, inProgress);
                implMembers.Append(deserializeMembers);
                baseList = $" : global::Serde.ISerde<{receiverType.ToFqn()}>";
                break;
            default:
                throw ExceptionUtilities.Unreachable;
        }

        var newType = new SourceBuilder("""

            #nullable enable

            using System;
            using Serde;

            """
        );

        if (receiverType.TypeKind == TypeKind.Enum)
        {
            var interfaceName = usage.GetInterfaceName();
            var providerString = $" : Serde.{interfaceName}<{receiverType.ToFqn()}>";
            typeDeclContext.AppendPartialDecl(newType, providerString, implMembers);
        }
        else
        {
            var objName = usage.GetSerdeObjName();
            var proxyType = new SourceBuilder($$"""
            sealed partial class {{objName}}{{baseList}}
            {
                global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => {{fullTypeString}}.s_serdeInfo;

                {{implMembers}}
            }
            """);
            typeDeclContext.AppendPartialDecl(newType, "", proxyType);
        }

        var srcName = fullTypeName + "." + usage.GetInterfaceName();
        context.AddSource(srcName, newType);
    }

    internal static (string FileName, SourceBuilder Decl) MakePartialDecl(
        TypeDeclContext typeDeclContext,
        string? baseList,
        SourceBuilder implMembers,
        string fileNameSuffix)
    {
        baseList ??= "";
        var nType = new SourceBuilder("""

        #nullable enable

        """);
        typeDeclContext.AppendPartialDecl(nType, baseList, implMembers);

        var srcName = typeDeclContext.GetFqn(includeTypeParameters: false) + "." + fileNameSuffix;
        return (srcName, nType);
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

        if (serdeSymbol is null)
        {
            return false;
        }

        if (targetType.AllInterfaces.Contains(serdeSymbol, SymbolEqualityComparer.Default)
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