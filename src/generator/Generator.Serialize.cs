using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;
using static Serde.WellKnownTypes;

namespace Serde;

public partial class SerializeImplGen
{
    internal static (List<MemberDeclarationSyntax>, BaseListSyntax) GenSerialize(
        TypeDeclContext typeDeclContext,
        GeneratorExecutionContext context,
        ITypeSymbol receiverType,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        var statements = new List<StatementSyntax>();
        var fieldsAndProps = SymbolUtilities.GetDataMembers(receiverType, SerdeUsage.Serialize);

        if (receiverType.TypeKind == TypeKind.Enum)
        {
            // For enums, the generated body should look like
            // ```
            // var typeInfo = {typeName}SerdeTypeInfo;
            // var index = value switch
            // {
            //   Enum.Case1 => 0,
            //   Enum.Case2 => 1,
            //   var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum '{TypeName}'");
            // };
            // serializer.SerializeEnumValue("Enum", name, (Underlying)value, default(Underlying));
            var enumType = (INamedTypeSymbol)receiverType;
            var typeSyntax = enumType.ToFqnSyntax();
            var underlying = enumType.EnumUnderlyingType!;
            statements.Add(ParseStatement($"var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<{typeDeclContext.Name}Proxy>();"));
            statements.Add(ParseStatement($$"""
            var index = value switch
            {
                {{string.Join("," + Utilities.NewLine, fieldsAndProps
                    .Select((m, i) => $"{typeSyntax}.{m.Name} => {i}")) }},
                var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum '{{enumType.Name}}'"),
            };
            """));
            var wrapper = Proxies.TryGetPrimitiveProxy(underlying, SerdeUsage.Serialize).NotNull().Proxy;
            statements.Add(ParseStatement(
                $"serializer.SerializeEnumValue(_l_serdeInfo, index, ({underlying.ToFqnSyntax()})value, {wrapper.ToFullString()}.Instance);"));
        }
        else
        {
            // The generated body of ISerialize is
            // `var _l_serdeInfo = {TypeName}SerdeTypeInfo.TypeInfo;`
            // `var type = serializer.SerializeType(_l_serdeInfo);
            // type.SerializeField<FieldType, Serialize>(_l_serdeInfo, FieldIndex, receiver.FieldValue);
            // type.End();

            // `var _l_serdeInfo = {TypeName}SerdeTypeInfo.TypeInfo;`
            statements.Add(ParseStatement($"var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<{typeDeclContext.Name}{typeDeclContext.TypeParameterList}>();"));

            // `var type = serializer.SerializeType(_l_serdeInfo);`
            statements.Add(ParseStatement("var type = serializer.SerializeType(_l_serdeInfo);"));

            for (int i = 0; i < fieldsAndProps.Count; i++)
            {
                var m = fieldsAndProps[i];
                // Generate statements of the form `type.SerializeField<FieldType, Serialize>("FieldName", value.FieldValue)`
                var typeAndWrapperOpt = MakeSerializeType(m, context, inProgress);
                if (typeAndWrapperOpt is not {} typeAndWrapper)
                {
                    // No built-in handling and doesn't implement ISerialize, error
                    context.ReportDiagnostic(CreateDiagnostic(
                        DiagId.ERR_DoesntImplementInterface,
                        m.Locations[0],
                        m.Symbol,
                        m.Type,
                        "Serde.ISerializeProvider<T>"));
                }
                else
                {
                    var memberExpr = MakeMemberAccessExpr(m, IdentifierName("value"));
                    statements.Add(MakeSerializeFieldStmt(m, i, memberExpr, typeAndWrapper, IdentifierName("value")));
                }
            }

            // `type.End();`
            statements.Add(ExpressionStatement(InvocationExpression(
                QualifiedName(IdentifierName("type"), IdentifierName("End")),
                ArgumentList()
            )));
        }

        var receiverSyntax = ((INamedTypeSymbol)receiverType).ToFqnSyntax();
        var receiverString = receiverType.ToDisplayString();

        string serdeInfoText = $"""
static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider<{receiverSyntax}>.SerdeInfo => {receiverSyntax}SerdeInfo.Instance;
""";
        // Generate method `void ISerialize<type>.Serialize(type value, ISerializer serializer) { ... }`
        List<MemberDeclarationSyntax> members = [
            MethodDeclaration(
                attributeLists: default,
                modifiers: default,
                PredefinedType(Token(SyntaxKind.VoidKeyword)),
                explicitInterfaceSpecifier: ExplicitInterfaceSpecifier(
                    GenericName(
                        Identifier("ISerialize"),
                        TypeArgumentList(SeparatedList(new TypeSyntax[] { receiverSyntax })))),
                identifier: Identifier("Serialize"),
                typeParameterList: null,
                parameterList: ParameterList(SeparatedList(new[] {
                    Parameter(receiverSyntax, "value"),
                    Parameter("ISerializer", "serializer")
                })),
                constraintClauses: default,
                body: Block(statements),
                expressionBody: null)
        ];
        List<BaseTypeSyntax> bases = [
            SimpleBaseType(ParseTypeName($"Serde.ISerialize<{receiverString}>")),
        ];
        if (receiverType.TypeKind == TypeKind.Enum)
        {
            bases.Add(SimpleBaseType(ParseTypeName($"Serde.ISerializeProvider<{receiverType.ToDisplayString()}>")));
            members.Add(ParseMemberDeclaration($$"""
            static ISerialize<{{receiverString}}> ISerializeProvider<{{receiverString}}>.SerializeInstance
                => {{receiverString}}Proxy.Instance;
            """)!);
        }
        return (members, BaseList(SeparatedList(bases)));

        // Make a statement like `type.SerializeField<valueType, SerializeType>("member.Name", value)`
        static ExpressionStatementSyntax MakeSerializeFieldStmt(
            DataMemberSymbol member,
            int index,
            ExpressionSyntax value,
            TypeWithProxy typeAndWrapper,
            ExpressionSyntax receiver)
        {
            var arguments = new List<ExpressionSyntax>() {
                    // _l_serdeInfo
                    ParseExpression("_l_serdeInfo"),
                    // Index
                    LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(index)),
                    // Value
                    value,
            };
            var typeArgs = new List<TypeSyntax>() {
                typeAndWrapper.Type,
                typeAndWrapper.Proxy
            };

            string methodName;
            if (member.IsNullable && !member.SerializeNull)
            {
                // Use SerializeFieldIfNotNull if it's not been disabled and the field is nullable
                methodName = "SerializeFieldIfNotNull";
            }
            else
            {
                methodName = "SerializeField";
            }

            return ExpressionStatement(InvocationExpression(
                // type.SerializeField
                QualifiedName(IdentifierName("type"),
                    GenericName(Identifier(methodName), TypeArgumentList(SeparatedList(typeArgs)))),
                ArgumentList(SeparatedList(arguments.Select(Argument)))));
        }
    }

    /// <summary>
    /// Constructs the argument to a ISerializer.Serialize call, i.e. constructs a term which
    /// implements ISerialize.  SerdeDn provides wrappers for primitives and common types in the
    /// framework. If found, we generate and initialize the wrapper.
    /// </summary>
    private static TypeWithProxy? MakeSerializeType(
        DataMemberSymbol member,
        GeneratorExecutionContext context,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        // 1. Check for an explicit wrapper
        if (Proxies.TryGetExplicitWrapper(member, context, SerdeUsage.Serialize, inProgress) is {} wrapper)
        {
            return new(member.Type.ToFqnSyntax(), wrapper);
        }

        // 2. Check for a direct implementation of ISerialize
        if (SerdeImplRoslynGenerator.ImplementsSerde(member.Type, member.Type, context, SerdeUsage.Serialize))
        {
            return new(member.Type.ToFqnSyntax(), member.Type.ToFqnSyntax());
        }

        // 3. A wrapper that implements ISerialize
        return Proxies.TryGetImplicitWrapper(member.Type, context, SerdeUsage.Serialize, inProgress);
    }

    private static ParameterSyntax Parameter(string typeName, string paramName, bool byRef = false) => SyntaxFactory.Parameter(
        attributeLists: default,
        modifiers: default,
        type: byRef ? SyntaxFactory.RefType(IdentifierName(typeName)) : IdentifierName(typeName),
        Identifier(paramName),
        default
    );

    private static ParameterSyntax Parameter(TypeSyntax typeSyntax, string paramName) => SyntaxFactory.Parameter(
        attributeLists: default,
        modifiers: default,
        type: typeSyntax,
        Identifier(paramName),
        default
    );

    private static MemberAccessExpressionSyntax MakeMemberAccessExpr(DataMemberSymbol m, ExpressionSyntax receiverExpr)
        => MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            receiverExpr,
            IdentifierName(m.Name));
}