
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;

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

                    var attributeData = attrCtx.Attributes.Single();

                    INamedTypeSymbol receiverType = typeSymbol;
                    // If the Through property is set, then we are implementing a wrapper type
                    if (attributeData.NamedArguments is [(nameof(GenerateSerialize.ThroughMember), { Value: string memberName })])
                    {
                        var members = model.LookupSymbols(typeDecl.SpanStart, typeSymbol, memberName);
                        if (members.Length != 1)
                        {
                            // TODO: Error about bad lookup
                            return;
                        }
                        receiverType = (INamedTypeSymbol)SymbolUtilities.GetSymbolType(members[0]);

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
                        Wrappers.GenerateEnumWrapper(
                            typeDecl,
                            attrCtx.SemanticModel,
                            generationContext);
                    }

                    SerdeTypeInfoGenerator.GenerateSerdeInfo(
                        typeDecl,
                        receiverType,
                        generationContext);

                    var inProgress = ImmutableList.Create<ITypeSymbol>(receiverType);

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
                    ctx.AddSource(fileName, content);
                }
            };

            context.RegisterSourceOutput(generateSerdeTypes, provideOutput);
            context.RegisterSourceOutput(combined, provideOutput);
        }
    }
}