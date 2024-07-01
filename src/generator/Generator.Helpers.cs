using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;
using static Serde.WellKnownTypes;

namespace Serde
{
    public partial class SerdeImplRoslynGenerator
    {
        // If the target is a core type, we can wrap it
        internal static NameSyntax? TryGetPrimitiveWrapper(ITypeSymbol type, SerdeUsage usage)
        {
            if (type.NullableAnnotation == NullableAnnotation.Annotated)
            {
                return null;
            }
            var name = type.SpecialType switch
            {
                SpecialType.System_Boolean => "BoolWrap",
                SpecialType.System_Char => "CharWrap",
                SpecialType.System_Byte => "ByteWrap",
                SpecialType.System_UInt16 => "UInt16Wrap",
                SpecialType.System_UInt32 => "UInt32Wrap",
                SpecialType.System_UInt64 => "UInt64Wrap",
                SpecialType.System_SByte => "SByteWrap",
                SpecialType.System_Int16 => "Int16Wrap",
                SpecialType.System_Int32 => "Int32Wrap",
                SpecialType.System_Int64 => "Int64Wrap",
                SpecialType.System_String => "StringWrap",
                SpecialType.System_Single => "FloatWrap",
                SpecialType.System_Double => "DoubleWrap",
                SpecialType.System_Decimal => "DecimalWrap",
                _ => null
            };
            return name is null ? null : IdentifierName(name);
        }

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
}