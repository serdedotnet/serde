using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;

namespace Serde
{
    /// <summary>
    /// Recognizes the [GenerateSerde] attribute on a type to generate an implementation
    /// of Serde.ISerialize. The implementation generally looks like a call to SerializeType,
    /// then successive calls to SerializeField.
    /// </summary>
    /// <example>
    /// For a type like,
    ///
    /// <code>
    /// [GenerateSerde]
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
    ///     void Serde.ISerialize.Serialize&lt;TSerializer, TSerializeType&gt;TSerializer serializer)
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
    public class IIncrementalSerializeGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            initContext.RegisterExecutionPipeline(context =>
            {
                // This should produce type symbols that have been annotated with GenerateSerdeAttribute
                var typeDefProvider = context.Sources.Syntax.Transform(
                    static node => ISerializeGenerator.IsGenerateSerdeAnnotated(node),
                    static ctx => ((TypeDeclarationSyntax)ctx.Node, ctx.SemanticModel)
                );

                typeDefProvider.GenerateSource((p, nodeTuple) =>
                {
                    var (typeDecl, typeDef) = nodeTuple;
                    ISerializeGenerator.GenerateImpl(new ISerializeGenerator.ProxyExecutionContext(p), typeDecl, typeDef);
                });
            });
        }
    }
}