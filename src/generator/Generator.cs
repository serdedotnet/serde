
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serde
{
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
    public partial class SerdeImplRoslynGenerator : IIncrementalGenerator
    {
        internal static string? SerdeBuiltInName(SpecialType specialType) => specialType switch
        {
            SpecialType.System_String => "String",
            SpecialType.System_Boolean => "Bool",
            SpecialType.System_Char => "Char",
            SpecialType.System_Byte => "Byte",
            SpecialType.System_UInt16 => "U16",
            SpecialType.System_UInt32 => "U32",
            SpecialType.System_UInt64 => "U64",
            SpecialType.System_SByte => "SByte",
            SpecialType.System_Int16 => "I16",
            SpecialType.System_Int32 => "I32",
            SpecialType.System_Int64 => "I64",
            SpecialType.System_Single => "Float",
            SpecialType.System_Double => "Double",
            SpecialType.System_Decimal => "Decimal",
            _ => null
        };

        /// <inheritdoc />
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            static GenerationOutput GenerateForCtx(
                SerdeUsage usage,
                GeneratorAttributeSyntaxContext attrCtx,
                CancellationToken cancelToken)
            {
                var generationContext = new GeneratorExecutionContext(attrCtx);
                var typeDecl = (BaseTypeDeclarationSyntax)attrCtx.TargetNode;
                if (typeDecl.IsKind(SyntaxKind.EnumDeclaration))
                {
                    GenerateEnumWrapper(
                        typeDecl,
                        attrCtx.SemanticModel,
                        generationContext);
                }
                if (usage.HasFlag(SerdeUsage.Serialize))
                {
                    SerdeImplRoslynGenerator.GenerateImpl(
                        SerdeUsage.Serialize,
                        (BaseTypeDeclarationSyntax)attrCtx.TargetNode,
                        attrCtx.SemanticModel,
                        generationContext,
                        ImmutableList<ITypeSymbol>.Empty);
                }
                if (usage.HasFlag(SerdeUsage.Deserialize))
                {
                    SerdeImplRoslynGenerator.GenerateImpl(
                        SerdeUsage.Deserialize,
                        (BaseTypeDeclarationSyntax)attrCtx.TargetNode,
                        attrCtx.SemanticModel,
                        generationContext,
                        ImmutableList<ITypeSymbol>.Empty);
                }
                return generationContext.GetOutput();
            }

            var generateSerdeTypes = context.SyntaxProvider.ForAttributeWithMetadataName(
                WellKnownAttribute.GenerateSerde.GetFqn(),
                (_, _) => true,
                (attrCtx, cancelToken) => GenerateForCtx(SerdeUsage.Both, attrCtx, cancelToken));
            var generateWrapperTypes = context.SyntaxProvider.ForAttributeWithMetadataName(
                WellKnownAttribute.GenerateWrapper.GetFqn(),
                (_, _) => true,
                (attrCtx, cancelToken) =>
                {
                    var generationContext = new GeneratorExecutionContext(attrCtx);
                    SerdeImplRoslynGenerator.GenerateWrapper(
                        generationContext,
                        attrCtx.Attributes.Single(),
                        (TypeDeclarationSyntax)attrCtx.TargetNode,
                        attrCtx.SemanticModel);
                    return generationContext.GetOutput();
                });

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
                    ctx.AddSource(fileName, content);
                }
            };

            context.RegisterSourceOutput(generateWrapperTypes, provideOutput);
            context.RegisterSourceOutput(generateSerdeTypes, provideOutput);
            context.RegisterSourceOutput(combined, provideOutput);
        }
    }
}