using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Serde
{
    [Generator]
    public class SerdeGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new TypeDeclReceiver());
        }
        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxReceiver = (TypeDeclReceiver)context.SyntaxContextReceiver!;
            var typeDecl = syntaxReceiver.TypeDeclarationSyntax;
            if (typeDecl is null)
            {
                return;
            }

            NamespaceDeclarationSyntax? namespaceDeclaration = null;
            for (SyntaxNode? current = typeDecl;
                 current is not null;
                 current = current.Parent)
            {
                if (current is NamespaceDeclarationSyntax ns)
                {
                    namespaceDeclaration = ns;
                }
            }
            var typeName = typeDecl.Identifier.ValueText;
            var semanticModel = syntaxReceiver.SemanticModel!;

            var typeDef = semanticModel.GetDeclaredSymbol(typeDecl)!;
            var fieldsAndProps = typeDef.GetMembers().Where(m => m is {
                    DeclaredAccessibility: Accessibility.Public,
                    Kind: SymbolKind.Field or SymbolKind.Property,
                }).ToList();

            var calls = fieldsAndProps.Select(m => $@"          type.SerializeField(""{m.Name}"", {m.Name}); ");

            var typeText = SourceText.From($@"
using Serde;
partial {typeDecl.Keyword} {typeName}
{{
    public void Serialize<TSerializer, TSerializeStruct>(TSerializer serializer)
        where TSerializer : ISerializer<TSerializeStruct>
        where TSerializeStruct : ISerializeStruct
    {{
        var type = serializer.SerializeStruct(""{typeName}"", 3);
{string.Join(Environment.NewLine, calls)}
        type.End();
    }}
}}
");
        }

        struct TypeDeclReceiver : ISyntaxContextReceiver
        {
            public TypeDeclarationSyntax? TypeDeclarationSyntax { get; private set; }
            public SemanticModel? SemanticModel { get; private set; }

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                var node = context.Node;
                if (node is TypeDeclarationSyntax typeDecl &&
                    (node.IsKind(SyntaxKind.ClassDeclaration)
                     || node.IsKind(SyntaxKind.StructDeclaration)
                     || node.IsKind(SyntaxKind.RecordDeclaration)))
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
                            if (name is IdentifierNameSyntax { Identifier: { ValueText: string text } }
                                && text == nameof(SerdeGenerator))
                            {
                                TypeDeclarationSyntax = typeDecl;
                                SemanticModel = context.SemanticModel;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}