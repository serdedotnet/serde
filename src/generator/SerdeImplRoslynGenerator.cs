
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StaticCs;
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
        // foreignType is non-null only when we have a non-empty proxy with an explicit
        // conversion. It represents the target type for the interface signatures and the
        // final conversion step. The rest of the pipeline operates on receiverType (the proxy).
        INamedTypeSymbol? foreignType = null;

        if (proxiedOpt is { } proxiedType)
        {
            if (proxiedType.SpecialType != SpecialType.None)
            {
                generationContext.ReportDiagnostic(CreateDiagnostic(
                    DiagId.ERR_CantWrapSpecialType,
                    attributeData.ApplicationSyntaxReference!.GetSyntax().GetLocation(),
                    proxiedType));
                return;
            }

            if (SymbolUtilities.GetDataMembers(typeSymbol, SerdeUsage.Both).Count != 0)
            {
                // Non-empty proxy: it is a representation (surrogate) of the foreign
                // type. We deserialize the proxy and convert it to the foreign type,
                // and convert the foreign type to the proxy to serialize. This requires
                // explicit conversion operators in both directions (depending on usage).
                foreignType = proxiedType;

                var conversionLocation = attributeData.ApplicationSyntaxReference!.GetSyntax().GetLocation();

                // Deserialize converts the deserialized proxy to the foreign type.
                if ((usage & SerdeUsage.Deserialize) != 0
                    && !HasExplicitConversion(typeSymbol, fromType: typeSymbol, toType: proxiedType))
                {
                    generationContext.ReportDiagnostic(CreateDiagnostic(
                        DiagId.ERR_MissingExplicitConversion,
                        conversionLocation,
                        typeSymbol, proxiedType));
                    return;
                }

                // Serialize converts the foreign value to the proxy before writing.
                if ((usage & SerdeUsage.Serialize) != 0
                    && !HasExplicitConversion(typeSymbol, fromType: proxiedType, toType: typeSymbol))
                {
                    generationContext.ReportDiagnostic(CreateDiagnostic(
                        DiagId.ERR_MissingReverseConversion,
                        conversionLocation,
                        typeSymbol, proxiedType));
                    return;
                }
            }
            else
            {
                // Empty proxy: receiverType becomes the foreign type
                receiverType = proxiedType;
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
            GenerateInfoAndSerdeImpls(usage, generationContext, typeDeclContext, receiverType, foreignType, inProgress);
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
            receiverType,
            foreignType);
    }

    internal static void GenerateProviderImpls(
        SerdeUsage usage,
        GeneratorExecutionContext generationContext,
        string serdeObjName,
        TypeDeclContext typeDeclContext,
        INamedTypeSymbol receiverType,
        INamedTypeSymbol? foreignType)
    {
        string fullTypeName = typeDeclContext.GetFqn(includeTypeParameters: false);

        string srcName;
        SourceBuilder content;
        content = GenProviderImplHelper(usage);
        srcName = $"{fullTypeName}.{usage.GetInterfaceName()}Provider";
        generationContext.AddSource(srcName, content);

        SourceBuilder GenProviderImplHelper(SerdeUsage usage)
            => GenProviderImpl(usage, serdeObjName, typeDeclContext, receiverType, foreignType);
    }

    private static SourceBuilder GenProviderImpl(
        SerdeUsage usage,
        string serdeObjName,
        TypeDeclContext typeDeclContext,
        ITypeSymbol receiverType,
        INamedTypeSymbol? foreignType
    )
    {
        // The provided type (the `T` in ISerdeProvider/I{Serialize,Deserialize}Provider)
        // is the type the SerdeObj actually implements serde for: the foreign type
        // when present, otherwise the receiver (proxy).
        var providedType = (ITypeSymbol?)foreignType ?? receiverType;
        var receiverString = providedType.ToDisplayString();
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
            baseList = $" : Serde.{interfaceName}<{providedType.ToFqn()}>";
            members = new SourceBuilder($$"""
            static global::Serde.{{usage.GetInterfaceName()}}<{{providedType.ToDisplayString()}}> global::Serde.{{interfaceName}}<{{providedType.ToFqn()}}>.Instance { get; }
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
        INamedTypeSymbol? foreignType,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress
    )
    {
        SerdeInfoGenerator.GenerateSerdeInfo(
            typeDeclContext,
            receiverType,
            foreignType,
            generationContext,
            usage,
            inProgress
        );

        GenSerdeObjImpl(
            usage,
            typeDeclContext,
            receiverType,
            foreignType,
            generationContext,
            inProgress
        );
    }

    /// <summary>
    /// If the type has a `ForType` property then we are generating a proxy type and need to find
    /// the proxied type.
    /// </summary>
    /// <returns>
    /// Returns null if there is no `ForType` property.
    /// </returns>
    private static INamedTypeSymbol? TryGetProxiedType(AttributeData attributeData)
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
                        return null;
                    }
                    return namedTypeSymbol;
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
        INamedTypeSymbol? foreignType,
        GeneratorExecutionContext context,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        string fullTypeName = typeDeclContext.GetFqn(includeTypeParameters: false);
        var fullTypeString = typeDeclContext.GetFqn();

        // The interface type is the foreign type when present, otherwise the receiver.
        var interfaceType = (ITypeSymbol?)foreignType ?? receiverType;

        // Generate statements for the implementation
        SourceBuilder implMembers;
        string? baseList;
        switch (usage)
        {
            case SerdeUsage.Serialize:
                implMembers = SerializeImplGen.GenSerialize(context, receiverType, foreignType, inProgress);
                baseList = $" : Serde.ISerialize<{interfaceType.ToDisplayString()}>";
                break;
            case SerdeUsage.Deserialize:
                implMembers = DeserializeImplGen.GenDeserialize(context, receiverType, foreignType, inProgress);
                baseList = $" : Serde.IDeserialize<{interfaceType.ToDisplayString()}>";
                break;
            case SerdeUsage.Both:
                implMembers = SerializeImplGen.GenSerialize(context, receiverType, foreignType, inProgress);
                var deserializeMembers = DeserializeImplGen.GenDeserialize(context, receiverType, foreignType, inProgress);
                implMembers.Append(deserializeMembers);
                baseList = $" : global::Serde.ISerde<{interfaceType.ToFqn()}>";
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

    /// <summary>
    /// Check if <paramref name="declaringType"/> declares a public static explicit
    /// conversion operator that converts from <paramref name="fromType"/> to
    /// <paramref name="toType"/>.
    /// </summary>
    internal static bool HasExplicitConversion(INamedTypeSymbol declaringType, ITypeSymbol fromType, ITypeSymbol toType)
    {
        foreach (var m in declaringType.GetMembers())
        {
            if (m is IMethodSymbol
                {
                    MethodKind: MethodKind.Conversion,
                    Name: "op_Explicit",
                    DeclaredAccessibility: Accessibility.Public,
                    IsStatic: true,
                    Parameters: [{ Type: var paramType }],
                    ReturnType: var returnType,
                }
                && SymbolEqualityComparer.Default.Equals(paramType, fromType)
                && SymbolEqualityComparer.Default.Equals(returnType, toType))
            {
                return true;
            }
        }
        return false;
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
