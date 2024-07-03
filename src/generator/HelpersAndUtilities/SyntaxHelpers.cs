using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Serde;

public partial class SyntaxHelpers
{
    internal static ParameterSyntax Parameter(string typeName, string paramName, bool byRef = false) => SyntaxFactory.Parameter(
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

    internal static LiteralExpressionSyntax NumericLiteral(int num)
        => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(num));

    internal static LiteralExpressionSyntax StringLiteral(string text)
        => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(text));

    private static MemberAccessExpressionSyntax MakeMemberAccessExpr(DataMemberSymbol m, ExpressionSyntax receiverExpr)
        => MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            receiverExpr,
            IdentifierName(m.Name));
}