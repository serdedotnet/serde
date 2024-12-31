
using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Serde
{
    /// <summary>
    /// Provides context about a given type declaration, including what kind of typeDecl it is, and
    /// what types and namespaces it's contained in. It's useful for generating new partial
    /// declarations of a type, or new nested types inside it.
    /// </summary>
    internal sealed class TypeDeclContext
    {
        public SyntaxKind Kind { get; init; }
        public string Name { get; init; }
        public List<string> NamespaceNames { get; init; }
        public List<(string Name, SyntaxKind Kind)> ParentTypeInfo { get; init; }
        public TypeParameterListSyntax? TypeParameterList { get; init; }
        public BaseTypeDeclarationSyntax TypeDecl { get; }

        public TypeDeclContext(BaseTypeDeclarationSyntax typeDecl)
        {
            TypeDecl = typeDecl;
            Kind = typeDecl.Kind();
            Name = typeDecl.Identifier.ValueText;
            var nsNames = new List<string>();
            var parentTypeInfos = new List<(string Name, SyntaxKind Kind)>();
            for (var parent = typeDecl.Parent; parent is not null; parent = parent.Parent)
            {
                switch (parent)
                {
                    case FileScopedNamespaceDeclarationSyntax ns:
                        nsNames.Add(ns.Name.ToString());
                        break;
                    case NamespaceDeclarationSyntax ns:
                        nsNames.Add(ns.Name.ToString());
                        break;
                    case TypeDeclarationSyntax t:
                        parentTypeInfos.Add((t.Identifier.ValueText, t.Kind()));
                        break;
                }
            }
            nsNames.Reverse();
            parentTypeInfos.Reverse();
            NamespaceNames = nsNames;
            ParentTypeInfo = parentTypeInfos;
            TypeParameterList = typeDecl is TypeDeclarationSyntax derived
                ? derived.TypeParameterList
                : null;
        }

        public static TypeDeclContext FromFile(string content, string typeDeclName)
        {
            var tree = ParseCompilationUnit(content);
            var typeDecl = FindTypeDecl(tree.Members, typeDeclName) ?? throw new InvalidOperationException($"Type {typeDeclName} not found in file");
            if (typeDecl is null)
            {
                throw new InvalidOperationException($"Type {typeDeclName} not found in file");
            }
            return new TypeDeclContext(typeDecl);

            static TypeDeclarationSyntax? FindTypeDecl(SyntaxList<MemberDeclarationSyntax> members, string typeDeclName)
            {
                foreach (var member in members)
                {
                    switch (member)
                    {
                        case BaseNamespaceDeclarationSyntax ns:
                            return FindTypeDecl(ns.Members, typeDeclName);
                        case TypeDeclarationSyntax t:
                            if (t.Identifier.ValueText == typeDeclName)
                            {
                                return t;
                            }
                            return FindTypeDecl(t.Members, typeDeclName);
                        default:
                            throw new InvalidOperationException($"Unexpected declaration kind {member}");
                    }
                }
                return null;
            }
        }

        public SourceBuilder MakeNestedType(SourceBuilder newType)
        {
            // If the original type was in a namespace or type, put this decl in the same one
            for (int i = ParentTypeInfo.Count - 1; i >= 0; i--)
            {
                var (name, kind) = ParentTypeInfo[i];
                newType = new SourceBuilder($$"""
partial {{TypeKindToString(kind)}} {{name}}
{
    {{newType}}
}
""");
            }

            if (NamespaceNames.Count > 0)
            {
                newType = new SourceBuilder($"""
                namespace {string.Join(".", NamespaceNames)};

                {newType}
                """);
            }

            return newType;
        }

        public static string TypeKindToString(SyntaxKind kind) => kind switch
        {
            SyntaxKind.ClassDeclaration => "class",
            SyntaxKind.StructDeclaration => "struct",
            SyntaxKind.RecordDeclaration => "record",
            SyntaxKind.RecordStructDeclaration => "record struct",
            SyntaxKind.InterfaceDeclaration => "interface",
            _ => throw new ArgumentException("Unsupported type kind: " + kind),
        };

        public MemberDeclarationSyntax WrapNewType(MemberDeclarationSyntax newType)
        {
            // If the original type was in a namespace or type, put this decl in the same one
            for (int i = ParentTypeInfo.Count - 1; i >= 0; i--)
            {
                var (name, kind) = ParentTypeInfo[i];
                newType = TypeDeclaration(kind, Identifier(name))
                    .WithModifiers(TokenList(Token(SyntaxKind.PartialKeyword)))
                    .WithMembers(List(new[] { newType }));
            }
            for (int i = NamespaceNames.Count - 1; i >= 0; i--)
            {
                newType = NamespaceDeclaration(
                    IdentifierName(NamespaceNames[i]),
                    externs: default,
                    usings: default,
                    members: List(new[] { newType }));
            }

            return newType;
        }
    }
}