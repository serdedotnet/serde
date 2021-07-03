
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FsCheck;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Serde.Test
{
    public class JsonFsCheck
    {
        private static readonly EmitOptions s_emitOptions = new EmitOptions();

        /// <summary>
        /// Types made up of built-ins should serialize with the same output
        /// as System.Text.Json.
        /// </summary>
        [Fact]
        public async Task CheckPrimitiveEquivalents()
        {
            var typeDefs = Gen.Sample(4, 5, Gen.Sized(FsCheckGenerators.GenTypeDef));
            var (names, classDecls) = ToMembers(typeDefs);

            var serializeStatements = new List<string>();
            serializeStatements.Add("var results = new List<(string, string)>();");
            serializeStatements.Add("var options = new System.Text.Json.JsonSerializerOptions() { IncludeFields = true };");
            int index = 0;
            foreach (var n in names)
            {
                var localName = "c" + index;
                serializeStatements.Add($"var {localName} = new {n}();");
                serializeStatements.Add($@"results.Add(
(Serde.JsonSerializer.WriteToString({localName}),
 System.Text.Json.JsonSerializer.Serialize({localName}, options)));");
                index++;
            }
            serializeStatements.Add("return results;");

            var body = string.Join(Environment.NewLine, serializeStatements);
            var mainTree = SyntaxFactory.ParseSyntaxTree($@"
using System;
using System.Collections.Generic;

public static class Runner
{{
    public static List<(string Serde, string SystemText)> Run()
    {{
        {body}
    }}
}}");

            var allTypes = SyntaxTree(CompilationUnit(
                externs: default,
                usings: List(new [] {
                    UsingDirective(IdentifierName("System")),
                    UsingDirective(QualifiedName(
                        QualifiedName(IdentifierName("System"), IdentifierName("Collections")),
                        IdentifierName("Generic"))),
                    UsingDirective(IdentifierName("Serde"))
                    }),
                attributeLists: default,
                List(classDecls)).NormalizeWhitespace());

            var comp = CSharpCompilation.Create(
               Guid.NewGuid().ToString("N"),
               syntaxTrees: new[] { mainTree, allTypes },
               references: (await ReferenceAssemblies.Net.Net50.ResolveAsync(null, default))
                    .Append(MetadataReference.CreateFromFile(typeof(Serde.ISerialize).Assembly.Location)),
               new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var driver = CSharpGeneratorDriver.Create(new SerializeGenerator());
            driver.RunGeneratorsAndUpdateCompilation(
                comp,
                out var newComp,
                out var diagnostics);
            
            var peStream = new MemoryStream();
            var result = newComp.Emit(peStream,
                pdbStream: null,
                xmlDocumentationStream: null,
                win32Resources: null,
                manifestResources: null,
                options: s_emitOptions,
                cancellationToken: default);

            Assert.True(result.Success);
            var loaded = Assembly.Load(peStream.GetBuffer());
            var testResults = (List<(string Serde, string SystemText)>)loaded.GetType("Runner")!.GetMethod("Run")!.Invoke(null, null)!;
            foreach (var (serde, systemText) in testResults)
            {
                Assert.Equal(systemText, serde);
            }
        }

        // Tuns a list of test types into a list of type declarations and the name of
        // the "root" type. A test type starts with the root, and generates multiple classes
        // that are all referenced by the root.
        public static (IReadOnlyList<string> RootNames, IReadOnlyList<MemberDeclarationSyntax> ClassDecls) ToMembers(IEnumerable<TestType> typeDefs)
        {
            // Types have generated unique names, counting from a depth-first traversal
            int typeIndex = 0;
            var types = new List<MemberDeclarationSyntax>();
            var rootNames = new List<string>();
            foreach (var type in typeDefs)
            {
                rootNames.Add(AddType((TestTypeDef)type));
            }

            return (rootNames, types);

            string AddType(TestTypeDef typeDef)
            {
                var typeName = "Class" + typeIndex++;
                var fields = new List<MemberDeclarationSyntax>();
                int fieldIndex = 0;
                foreach (var field in typeDef.FieldTypes)
                {
                    TypeSyntax fieldTypeName;
                    ExpressionSyntax initializer;
                    switch (field)
                    {
                        case TestBool b:
                            fieldTypeName = b.TypeName;
                            initializer = LiteralExpression(SyntaxKind.FalseLiteralExpression);
                            break;
                        case TestPrimitive p:
                            fieldTypeName = p.TypeName;
                            initializer = MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                fieldTypeName,
                                IdentifierName("MaxValue"));
                            break;
                        case TestTypeDef d:
                            fieldTypeName = IdentifierName(AddType(d));
                            initializer = ObjectCreationExpression(
                                fieldTypeName,
                                argumentList: ArgumentList(),
                                initializer: null);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected TestType" + field);
                    }
                    fields.Add(FieldDeclaration(
                        attributeLists: default,
                        modifiers: TokenList(Token(SyntaxKind.PublicKeyword)),
                        VariableDeclaration(
                        fieldTypeName,
                        SeparatedList(new[] { VariableDeclarator(
                            Identifier("Field" + fieldIndex),
                            argumentList: null,
                            initializer: EqualsValueClause(initializer)) })
                    )));
                    fieldIndex++;
                }
                types.Add(ClassDeclaration(
                    attributeLists: List(new[] { AttributeList(SeparatedList(new[] { Attribute(IdentifierName("GenerateSerde")) })) }),
                    modifiers: TokenList(new[] { Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.PartialKeyword) }),
                    identifier: Identifier(typeName),
                    typeParameterList: null,
                    baseList: null,
                    constraintClauses: default,
                    List(fields)
                ));
                return typeName;
            }
        }

        public abstract record TestType;
        public abstract record TestPrimitive : TestType
        {
            public abstract TypeSyntax TypeName { get; }
        }
        public sealed record TestByte : TestPrimitive
        {
            public sealed override TypeSyntax TypeName => PredefinedType(Token(SyntaxKind.ByteKeyword));
        }
        public record TestBool : TestPrimitive
        {
            public sealed override TypeSyntax TypeName => PredefinedType(Token(SyntaxKind.BoolKeyword));
        }

        public record TestTypeDef(ImmutableArray<TestType> FieldTypes) : TestType;

        public static class FsCheckGenerators
        {
            public static Gen<TestType> GenPrimitive()
                => Gen.OneOf(new[] {
                    Gen.Constant<TestType>(new TestByte()),
                    Gen.Constant<TestType>(new TestBool())
                });

            public static Gen<TestType> GenType(int size)
            {
                var genPrim = GenPrimitive();
                if (size == 0)
                {
                    return genPrim;
                }
                else
                {
                    return Gen.OneOf(new[] {
                        genPrim,
                        GenTypeDef(size)
                    });
                }
            }

            public static Gen<TestType> GenTypeDef(int size)
            {
                Gen<TestType> genField = GenType(size / 2);
                return
                    // generate classes with between 1 and 3 fields
                    from arraySize in Gen.Choose(1, 3)
                    from array in Gen.ArrayOf(arraySize, genField)
                    select (TestType)new TestTypeDef(array.ToImmutableArray());
            }
        }
    }
}