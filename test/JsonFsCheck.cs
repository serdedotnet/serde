
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
        public async Task CheckPrimitiveEquivalentsAsync()
        {
            // Generates test cases, each of which has multiple generated classes
            var testCases = Gen.Sample(4, 100, Gen.Sized(FsCheckGenerators.GenTypeDef));
            var wrappers = new MemberDeclarationSyntax[testCases.Length];
            int wrapperIndex = 0;
            foreach (var type in testCases)
            {
                var classDecls = ToMembers((TestTypeDef)type);
                // Wrap the classes used by each test case in an outer class to
                // prevent name collision
                wrappers[wrapperIndex] = ClassDeclaration(
                    attributeLists: default,
                    modifiers: TokenList(Token(SyntaxKind.PartialKeyword)),
                    identifier: Identifier("TestCase" + wrapperIndex),
                    typeParameterList: null,
                    baseList: null,
                    constraintClauses: default,
                    members: List(classDecls)).NormalizeWhitespace();
                wrapperIndex++;
            }

            var serializeStatements = new List<string>();
            serializeStatements.Add("var results = new List<(string, string)>();");
            serializeStatements.Add("var options = new System.Text.Json.JsonSerializerOptions() { IncludeFields = true };");
            for (int i = 0; i < wrappers.Length; i++)
            {
                var localName = "t" + i;
                serializeStatements.Add($"var {localName} = new TestCase{i}.Class0();");
                serializeStatements.Add($@"results.Add(
(Serde.JsonSerializer.Serialize({localName}),
 System.Text.Json.JsonSerializer.Serialize({localName}, options)));");
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
                List(wrappers)).NormalizeWhitespace());

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

            Assert.Empty(diagnostics);
            
            var peStream = new MemoryStream();
            var result = newComp.Emit(peStream,
                pdbStream: null,
                xmlDocumentationStream: null,
                win32Resources: null,
                manifestResources: null,
                options: s_emitOptions,
                cancellationToken: default);

            Assert.True(result.Success, allTypes.ToString());
            var loaded = Assembly.Load(peStream.GetBuffer());
            var testResults = (List<(string Serde, string SystemText)>)loaded.GetType("Runner")!.GetMethod("Run")!.Invoke(null, null)!;
            foreach (var (serde, systemText) in testResults)
            {
                Assert.Equal(systemText, serde);
            }
        }

        // Tuns a test type into a list of type declarations. The root type is always named
        // "Class0".
        public static IReadOnlyList<MemberDeclarationSyntax> ToMembers(TestTypeDef typeDef)
        {
            // Types have generated unique names, counting from a depth-first traversal
            var types = new List<MemberDeclarationSyntax>();
            _ = VisitType(typeDef, typeIndex: 0, types);
            // Reverse types, as they're added depth-first
            types.Reverse();
            return types;

            // Generate syntax for the given typedef, and the type definition for the generated
            // types of any fields. Returns the type index of the last type generated.
            static int VisitType(TestType type, int typeIndex, List<MemberDeclarationSyntax> types)
            {
                switch (type)
                {
                    case TestListLike arr:
                        return VisitType(arr.ElementType, typeIndex, types);
                    case TestTypeDef typeDef:
                        var fieldsSyntax = new List<MemberDeclarationSyntax>();
                        // nextIndex tracks the number of types we're generating. Every type
                        // will get a new index, which will be used to create a unique name
                        int nextIndex = typeIndex + 1;
                        for (int fieldIndex = 0; fieldIndex < typeDef.FieldTypes.Length; fieldIndex++)
                        {
                            var field = typeDef.FieldTypes[fieldIndex];
                            var fieldTypeName = field.TypeSyntax(nextIndex);
                            ExpressionSyntax initializer = field.Value(nextIndex);
                            fieldsSyntax.Add(FieldDeclaration(
                                attributeLists: default,
                                modifiers: TokenList(Token(SyntaxKind.PublicKeyword)),
                                VariableDeclaration(
                                    fieldTypeName,
                                    SeparatedList(new[] { VariableDeclarator(
                                Identifier("Field" + fieldIndex),
                                argumentList: null,
                                initializer: EqualsValueClause(initializer)) }))
                            ));
                            nextIndex = VisitType(field, nextIndex, types);
                        }

                        // Add the first type
                        types.Add(ClassDeclaration(
                            attributeLists: List(new[] { AttributeList(SeparatedList(new[] { Attribute(IdentifierName("GenerateISerialize")) })) }),
                            modifiers: TokenList(new[] { Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.PartialKeyword) }),
                            identifier: Identifier(typeDef.TypeName(typeIndex)),
                            typeParameterList: null,
                            baseList: null,
                            constraintClauses: default,
                            List(fieldsSyntax)
                        ));
                        return nextIndex;

                    default:
                        return typeIndex;
                }
            }
        }

        public abstract record TestType
        {
            public abstract TypeSyntax TypeSyntax(int typeIndex);
            public abstract ExpressionSyntax Value(int typeIndex);
        }
        public abstract record TestPrimitive : TestType
        {
            public abstract SyntaxKind SyntaxKind { get; }
            public sealed override TypeSyntax TypeSyntax(int typeIndex) => PredefinedType(Token(SyntaxKind));
            public override ExpressionSyntax Value(int typeIndex) => MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                TypeSyntax(typeIndex),
                IdentifierName("MaxValue"));

        }
        public sealed record TestByte : TestPrimitive
        {
            public sealed override SyntaxKind SyntaxKind => SyntaxKind.ByteKeyword;
        }
        public sealed record TestChar : TestPrimitive
        {
            public sealed override SyntaxKind SyntaxKind => SyntaxKind.CharKeyword;
        }
        public sealed record TestBool : TestPrimitive
        {
            public sealed override SyntaxKind SyntaxKind => SyntaxKind.BoolKeyword;
            public override ExpressionSyntax Value(int typeIndex) => LiteralExpression(SyntaxKind.FalseLiteralExpression);
        }
        public sealed record TestInt : TestPrimitive
        {
            public sealed override SyntaxKind SyntaxKind => SyntaxKind.IntKeyword;
        }
        public record TestTypeDef(ImmutableArray<TestType> FieldTypes) : TestType
        {
            public override TypeSyntax TypeSyntax(int typeIndex) => IdentifierName(TypeName(typeIndex));
            public override ExpressionSyntax Value(int typeIndex) => ObjectCreationExpression(
                TypeSyntax(typeIndex),
                argumentList: ArgumentList(),
                initializer: null);
            public string TypeName(int typeIndex) => "Class" + typeIndex;
        }
        public abstract record TestListLike(int Length, TestType ElementType) : TestType
        {
            protected ExpressionSyntax[] MakeElements(int typeIndex)
            {
                var values = new ExpressionSyntax[Length];
                for (int i = 0; i < Length; i++)
                {
                    values[i] = ElementType.Value(typeIndex);
                }
                return values;
            }
        }
        public record TestTypeArray(int Length, TestType ElementType) : TestListLike(Length, ElementType)
        {
            public override ArrayTypeSyntax TypeSyntax(int typeIndex)
                => ArrayType(
                    ElementType.TypeSyntax(typeIndex),
                    List(new[] { 
                        ArrayRankSpecifier(SeparatedList(new ExpressionSyntax[] { OmittedArraySizeExpression() }))
                   }));

            public override ExpressionSyntax Value(int typeIndex)
            {
                return ArrayCreationExpression(
                    TypeSyntax(typeIndex),
                    initializer: InitializerExpression(
                        SyntaxKind.ArrayInitializerExpression,
                        SeparatedList(MakeElements(typeIndex))));
            }
        }

        public record TestTypeList(int Length, TestType ElementType) : TestListLike(Length, ElementType)
        {
            public override TypeSyntax TypeSyntax(int typeIndex)
                => GenericName(Identifier("List"), TypeArgumentList(SeparatedList(new[] { ElementType.TypeSyntax(typeIndex) })));
                
            public override ExpressionSyntax Value(int typeIndex)
                => ObjectCreationExpression(
                    TypeSyntax(typeIndex),
                    argumentList: ArgumentList(),
                    initializer: InitializerExpression(SyntaxKind.CollectionInitializerExpression, SeparatedList(MakeElements(typeIndex)))
                    );
        }

        public static class FsCheckGenerators
        {
            public static Gen<TestType> GenPrimitive { get; } = Gen.OneOf(new[] {
                    Gen.Constant<TestType>(new TestChar()),
                    Gen.Constant<TestType>(new TestBool()),
                    Gen.Constant<TestType>(new TestInt())
                });

            public static Gen<TestType> GenType(int size)
            {
                if (size == 0)
                {
                    return GenPrimitive;
                }
                else
                {
                    var genAny = Gen.OneOf(new[] {
                        GenPrimitive,
                        GenTypeArray(size),
                        GenTypeList(size),
                        GenTypeDef(size)
                    });
                    return genAny;
                }
            }

            public static Gen<(int Length, TestType ElementType)> GenListLike(int size)
                // generate between 0 and 2 elements
                => Gen.Choose(0, 2).Zip(GenType(size / 2), (l, t) => (l, t));

            public static Gen<TestType> GenTypeArray(int size)
                => GenListLike(size).Select(x => (TestType)new TestTypeArray(x.Length, x.ElementType));

            public static Gen<TestType> GenTypeList(int size)
                => GenListLike(size).Select(x => (TestType)new TestTypeList(x.Length, x.ElementType));

            public static Gen<TestType> GenTypeDef(int size)
            {
                // generate class with between 1 and 3 fields
                return Gen.Choose(1, 3)
                    .SelectMany(arraySize => ImmArrayOf(arraySize, GenType(size / 2))
                        .Select(types => ((TestType)new TestTypeDef(types))));
            }

            public static Gen<ImmutableArray<T>> ImmArrayOf<T>(int n, Gen<T> gen)
                => Gen.ArrayOf(n, gen).Select(a => a.ToImmutableArray());
        }
    }
}