
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Serde.Diagnostics;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Serde
{
    partial class Generator
    {
        internal void GenerateWrapper(
            GeneratorExecutionContext context,
            AttributeSyntax attributeSyntax,
            TypeDeclarationSyntax typeDecl,
            SemanticModel model)
        {
            // Type must be partial
            if (!typeDecl.Modifiers.Any(tok => tok.IsKind(SyntaxKind.PartialKeyword)))
            {
                context.ReportDiagnostic(CreateDiagnostic(
                    DiagId.ERR_TypeNotPartial,
                    typeDecl.Identifier.GetLocation(),
                    typeDecl.Identifier.ValueText));
                return;
            }

            var attrArgSyntax = attributeSyntax.ArgumentList?.Arguments[0];
            if (attrArgSyntax is null)
            {
                return;
            }
            var argValue = model.GetConstantValue(attrArgSyntax.Expression);
            if (argValue.Value is not string memberName)
            {
                return;
            }

            var containerSymbol = model.GetDeclaredSymbol(typeDecl)!;
            ExpressionSyntax receiverExpr;
            ITypeSymbol receiverType;
            var members = model.LookupSymbols(typeDecl.SpanStart, containerSymbol, memberName);
            if (members.Length != 1)
            {
                // Error about bad lookup
                return;
            }
            receiverType = SymbolUtilities.GetSymbolType(members[0]);
            receiverExpr = IdentifierName(memberName);

            if (receiverType.SpecialType != SpecialType.None)
            {
                context.ReportDiagnostic(CreateDiagnostic(
                    DiagId.ERR_CantWrapSpecialType,
                    attrArgSyntax.GetLocation(),
                    receiverType));
                return;
            }

            GenerateSerialize(context, typeDecl, model, receiverType, receiverExpr);
            GenerateDeserialize(context, typeDecl, model, receiverType, receiverExpr);
        }
    }

    internal enum WrapUsage : byte
    {
        Serialize = 0b01,
        Deserialize = 0b10,
    }

    internal static class WrapUsageExtensions
    {
        public static string GetName(this WrapUsage usage) => usage switch
        {
            WrapUsage.Serialize => "SerializeImpl",
            WrapUsage.Deserialize => "DeserializeImpl",
            _ => throw ExceptionUtilities.Unreachable
        };
    }
}