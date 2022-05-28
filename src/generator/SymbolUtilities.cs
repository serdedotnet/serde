
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serde
{
    internal static class SymbolUtilities
    {
        public static ITypeSymbol GetSymbolType(ISymbol symbol) => symbol switch
        {
            IFieldSymbol f => f.Type,
            IPropertySymbol p => p.Type,
            IParameterSymbol p => p.Type,
            ILocalSymbol l => l.Type,
            _ => throw new InvalidOperationException($"Unexpected symbol {symbol}")
        };

        public static List<DataMemberSymbol> GetPublicDataMembers(ITypeSymbol type)
        {
            var format = GetTypeOptions(type).MemberFormat;
            return type.GetMembers()
                .Where(m => m is {
                    DeclaredAccessibility: Accessibility.Public,
                    Kind: SymbolKind.Field or SymbolKind.Property,
                })
                .Select(m => new DataMemberSymbol(m, format)).ToList();
        }

        public static TypeSyntax ToFqnSyntax(this INamedTypeSymbol t) => SyntaxFactory.ParseTypeName(t.ToDisplayString());

        internal static TypeOptions GetTypeOptions(ITypeSymbol type)
        {
            var options = new TypeOptions();
            foreach (var attr in type.GetAttributes())
            {
                var attrClass = attr.AttributeClass;
                if (attrClass is null)
                {
                    continue;
                }
                if (WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.SerdeTypeOptions))
                {
                    foreach (var named in attr.NamedArguments)
                    {
                        var value = named.Value.Value!;
                        switch (named)
                        {
                            case { Key: "MemberFormat",
                                Value: {
                                    Kind: TypedConstantKind.Enum,
                                    Type: { Name: "MemberFormat" }
                                } }:
                                options = options with {
                                    MemberFormat = (MemberFormat)value
                                };
                                break;
                            case { Key: "DenyUnknownMembers",
                                Value: {
                                    Kind: TypedConstantKind.Primitive,
                                    Type: { SpecialType: SpecialType.System_Boolean }
                                }}:
                                options = options with {
                                    DenyUnknownMembers = (bool)value
                                };
                                break;
                        }
                    }
                    break;
                }
            }
            return options;
        }

    }
}