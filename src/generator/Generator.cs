
using System.Linq;
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

    public partial class Generator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            foreach (var tree in context.Compilation.SyntaxTrees)
            {
                foreach (var typeDecl in tree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>())
                {
                    if (typeDecl.IsKind(SyntaxKind.ClassDeclaration)
                         || typeDecl.IsKind(SyntaxKind.StructDeclaration)
                         || typeDecl.IsKind(SyntaxKind.RecordDeclaration))
                    {
                        foreach (var attrLists in typeDecl.AttributeLists)
                        {
                            foreach (var attr in attrLists.Attributes)
                            {
                                var name = attr.Name;
                                while (name is QualifiedNameSyntax q)
                                {
                                    name = q.Right;
                                }
                                switch (name)
                                {
                                    case IdentifierNameSyntax { Identifier:
                                        {
                                            ValueText: "GenerateSerialize" or "GenerateSerializeAttribute"
                                        }}:
                                        GenerateSerialize(context, typeDecl, context.Compilation.GetSemanticModel(tree));
                                        break;
                                    case IdentifierNameSyntax { Identifier:
                                        {
                                            ValueText: "GenerateWrapper" or "GenerateWrapperAttribute"
                                        }}:
                                        GenerateWrapper(context, attr, typeDecl, context.Compilation.GetSemanticModel(tree));
                                        break;
                                }
                            }
                        }
                    }

                }
            }
        }
    }
}