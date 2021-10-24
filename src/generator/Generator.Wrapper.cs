
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

            GenerateSerialize(context, typeDecl, model, memberName);
        }

        private static string? TryGetBuiltInName(ITypeSymbol type) => type.SpecialType switch
        {
            SpecialType.System_String => "String",
            _ => null
        };
    }
}