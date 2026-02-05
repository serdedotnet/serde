
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serde
{
    /// <summary>
    /// Represents a member with no arguments, namely a field or property
    /// </summary>
    internal readonly struct DataMemberSymbol
    {
        private readonly TypeOptions _typeOptions;
        private readonly MemberOptions _memberOptions;

        /// <summary>
        /// The field or property may contain null.
        /// </summary>
        public bool IsNullable { get; }
        public ISymbol Symbol { get; }

        public DataMemberSymbol(
            ISymbol symbol,
            TypeOptions typeOptions,
            MemberOptions memberOptions)
        {
            Debug.Assert(symbol is
                IFieldSymbol or
                IPropertySymbol { Parameters.Length: 0 });
            Symbol = symbol;
            _typeOptions = typeOptions;
            _memberOptions = memberOptions;
            this.IsNullable = IsNullable(symbol);

            // Assumes that the symbol is in a nullable-enabled context, and lack of annotation
            // means not-nullable
            static bool IsNullable(ISymbol symbol)
            {
                var type = GetType(symbol);
                if (type.NullableAnnotation == NullableAnnotation.Annotated ||
                    type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
                {
                    return true;
                }

                if (type.IsValueType)
                {
                    return false;
                }

                if (type is ITypeParameterSymbol param)
                {
                    if (param.HasNotNullConstraint)
                    {
                        return false;
                    }
                    foreach (var ann in param.ConstraintNullableAnnotations)
                    {
                        if (ann == NullableAnnotation.Annotated)
                        {
                            return true;
                        }
                    }
                    if (param.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.Annotated)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public ITypeSymbol Type => GetType(Symbol);

        private static ITypeSymbol GetType(ISymbol symbol) => symbol switch
        {
            IFieldSymbol f => f.Type,
            IPropertySymbol p => p.Type,
            _ => throw ExceptionUtilities.Unreachable
        };

        public NullableAnnotation NullableAnnotation => Symbol switch
        {
            IFieldSymbol f => f.NullableAnnotation,
            IPropertySymbol p => p.NullableAnnotation,
            _ => throw ExceptionUtilities.Unreachable
        };

        public ImmutableArray<Location> Locations => Symbol.Locations;

        public string Name => Symbol.Name;

        public bool SkipDeserialize => _memberOptions.SkipDeserialize;

        public bool SkipSerialize => _memberOptions.SkipSerialize;

        public bool? ThrowIfMissing => _memberOptions.ThrowIfMissing;

        public bool SerializeNull => _memberOptions.SerializeNull ?? _typeOptions.SerializeNull;

        public ImmutableArray<AttributeData> Attributes => Symbol.GetAttributes();

        /// <summary>
        /// Retrieves the name of the member after formatting options are applied.
        /// </summary>
        public string GetFormattedName()
        {
            if (_memberOptions.Rename is { } renamed)
            {
                return renamed;
            }
            if (_typeOptions.MemberFormat == MemberFormat.None)
            {
                return Name;
            }
            var parts = ParseMemberName(Name);
            switch (_typeOptions.MemberFormat)
            {
                case MemberFormat.CamelCase:
                    {
                        var builder = new StringBuilder();
                        bool first = true;
                        foreach (var part in parts)
                        {
                            if (first)
                            {
                                builder.Append(char.ToLowerInvariant(part[0]));
                                first = false;
                            }
                            else
                            {
                                builder.Append(char.ToUpperInvariant(part[0]));
                            }
                            builder.Append(part.Substring(1).ToLowerInvariant());
                        }
                        return builder.ToString();
                    }
                case MemberFormat.PascalCase:
                    {
                        var builder = new StringBuilder();
                        foreach (var part in parts)
                        {
                            builder.Append(char.ToUpperInvariant(part[0]));
                            builder.Append(part.Substring(1).ToLowerInvariant());
                        }
                        return builder.ToString();
                    }
                case MemberFormat.KebabCase:
                    return string.Join("-", parts.Select(s => s.ToLowerInvariant()));

                default:
                    throw new InvalidOperationException("Invalid member format: " + _typeOptions.MemberFormat);
            }
        }

        private static ImmutableArray<string> ParseMemberName(string name)
        {
            var resultBuilder = ImmutableArray.CreateBuilder<string>();
            var wordBuilder = new StringBuilder();
            foreach (var c in name)
            {
                if (c == '_')
                {
                    AddWordAndClear();
                    continue;
                }
                if (char.IsUpper(c))
                {
                    AddWordAndClear();
                }
                wordBuilder.Append(c);
            }
            AddWordAndClear();
            return resultBuilder.ToImmutable();

            void AddWordAndClear()
            {
                if (wordBuilder.Length > 0)
                {
                    resultBuilder.Add(wordBuilder.ToString());
                    wordBuilder.Clear();
                }
            }
        }

        /// <summary>
        /// Returns the initializer expression text for the field/property with all symbols fully qualified,
        /// or null if there is no initializer or the initializer cannot be safely preserved.
        /// </summary>
        public string? GetInitializer(Compilation compilation)
        {
            foreach (var syntaxRef in Symbol.DeclaringSyntaxReferences)
            {
                var syntax = syntaxRef.GetSyntax();
                EqualsValueClauseSyntax? initializer = syntax switch
                {
                    VariableDeclaratorSyntax v => v.Initializer,
                    PropertyDeclarationSyntax p => p.Initializer,
                    _ => null
                };
                if (initializer is not null)
                {
                    var semanticModel = compilation.GetSemanticModel(syntax.SyntaxTree);
                    var qualifiedExpr = QualifyExpression(initializer.Value, semanticModel);
                    // If the rewriter returned null, the expression can't be safely preserved
                    if (qualifiedExpr is null)
                    {
                        return null;
                    }
                    return qualifiedExpr.ToFullString();
                }
            }
            return null;
        }

        /// <summary>
        /// Rewrites an expression to fully qualify all type references.
        /// Returns null if the expression cannot be safely preserved.
        /// </summary>
        private static ExpressionSyntax? QualifyExpression(ExpressionSyntax expr, SemanticModel semanticModel)
        {
            var rewriter = new FullyQualifyingRewriter(semanticModel);
            var result = rewriter.Visit(expr);
            if (rewriter.Failed)
            {
                return null;
            }
            return (ExpressionSyntax?)result;
        }

        /// <summary>
        /// Format for fully qualified symbol names with global:: prefix.
        /// </summary>
        private static readonly SymbolDisplayFormat s_fullyQualifiedFormat = SymbolDisplayFormat.FullyQualifiedFormat
            .WithMemberOptions(SymbolDisplayMemberOptions.IncludeContainingType);

        /// <summary>
        /// A syntax rewriter that fully qualifies all type and member references.
        /// Sets Failed to true if the expression cannot be safely preserved.
        /// </summary>
        private sealed class FullyQualifyingRewriter : CSharpSyntaxRewriter
        {
            private readonly SemanticModel _semanticModel;

            public bool Failed { get; private set; }

            public FullyQualifyingRewriter(SemanticModel semanticModel)
            {
                _semanticModel = semanticModel;
            }

            public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
            {
                var symbolInfo = _semanticModel.GetSymbolInfo(node);
                if (symbolInfo.Symbol is ITypeSymbol typeSymbol)
                {
                    var fqn = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    return SyntaxFactory.ParseTypeName(fqn).WithTriviaFrom(node);
                }
                return base.VisitIdentifierName(node);
            }

            public override SyntaxNode? VisitPredefinedType(PredefinedTypeSyntax node)
            {
                // Predefined types like 'string', 'int' are always in scope - no need to qualify
                return base.VisitPredefinedType(node);
            }

            public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
            {
                var symbolInfo = _semanticModel.GetSymbolInfo(node);
                var symbol = symbolInfo.Symbol;

                // Handle static field/property/method access like string.Empty or Console.WriteLine
                if (symbol is IFieldSymbol { IsStatic: true } or
                    IPropertySymbol { IsStatic: true } or
                    IMethodSymbol { IsStatic: true })
                {
                    // Use ToDisplayString to get fully qualified member access
                    var fqn = symbol.ToDisplayString(s_fullyQualifiedFormat);
                    return SyntaxFactory.ParseExpression(fqn).WithTriviaFrom(node);
                }

                // For instance member access, just qualify the expression part
                return base.VisitMemberAccessExpression(node);
            }

            public override SyntaxNode? VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                var symbolInfo = _semanticModel.GetSymbolInfo(node);

                // Check if this is an extension method call
                if (symbolInfo.Symbol is IMethodSymbol { IsExtensionMethod: true } method)
                {
                    // Rewrite extension method call to explicit static call
                    // e.g., list.ToImmutableArray() -> ImmutableArray.ToImmutableArray(list)
                    var containingType = method.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    var methodName = method.Name;

                    // Get the receiver (the 'this' argument) from the member access
                    ExpressionSyntax? receiver = null;
                    if (node.Expression is MemberAccessExpressionSyntax memberAccess)
                    {
                        receiver = (ExpressionSyntax?)Visit(memberAccess.Expression);
                    }

                    // Build the argument list: receiver + original arguments
                    var arguments = new List<ArgumentSyntax>();
                    if (receiver is not null)
                    {
                        arguments.Add(SyntaxFactory.Argument(receiver));
                    }
                    foreach (var arg in node.ArgumentList.Arguments)
                    {
                        var visitedArg = (ArgumentSyntax?)Visit(arg) ?? arg;
                        arguments.Add(visitedArg);
                    }

                    // Handle generic method type arguments
                    SimpleNameSyntax methodNameSyntax;
                    if (method.TypeArguments.Length > 0)
                    {
                        var typeArgs = method.TypeArguments
                            .Select(t => SyntaxFactory.ParseTypeName(t.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)))
                            .ToArray();
                        methodNameSyntax = SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier(methodName),
                            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(typeArgs)));
                    }
                    else
                    {
                        methodNameSyntax = SyntaxFactory.IdentifierName(methodName);
                    }

                    // Build: ContainingType.MethodName(receiver, args...)
                    var qualifiedMethod = SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.ParseTypeName(containingType),
                        methodNameSyntax);

                    return SyntaxFactory.InvocationExpression(
                        qualifiedMethod,
                        SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(arguments)))
                        .WithTriviaFrom(node);
                }

                return base.VisitInvocationExpression(node);
            }

            public override SyntaxNode? VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
            {
                // If there's a complex initializer (collection init, etc.), don't preserve
                if (node.Initializer is not null)
                {
                    Failed = true;
                    return node;
                }

                var typeInfo = _semanticModel.GetTypeInfo(node);
                if (typeInfo.Type is INamedTypeSymbol namedType)
                {
                    // Only preserve object creation if:
                    // 1. It has arguments (constructor with params)
                    // 2. It's a value type (structs always have parameterless constructors)
                    // 3. It has a parameterless constructor
                    bool hasArgs = node.ArgumentList?.Arguments.Count > 0;
                    bool isValueType = namedType.IsValueType;
                    bool hasParameterlessCtor = namedType.InstanceConstructors
                        .Any(c => c.Parameters.Length == 0 && c.DeclaredAccessibility == Accessibility.Public);

                    if (!hasArgs && !isValueType && !hasParameterlessCtor)
                    {
                        // Can't safely preserve this - the type doesn't have a parameterless constructor
                        Failed = true;
                        return node;
                    }

                    var fqn = namedType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    var qualifiedType = SyntaxFactory.ParseTypeName(fqn)
                        .WithLeadingTrivia(node.Type.GetLeadingTrivia());

                    var newArgs = node.ArgumentList is not null
                        ? (ArgumentListSyntax?)Visit(node.ArgumentList)
                        : null;

                    return SyntaxFactory.ObjectCreationExpression(
                        node.NewKeyword,
                        qualifiedType,
                        newArgs,
                        null);
                }
                return base.VisitObjectCreationExpression(node);
            }

            public override SyntaxNode? VisitImplicitObjectCreationExpression(ImplicitObjectCreationExpressionSyntax node)
            {
                // Target-typed new like: new() { ... } or new(args)
                // These are complex to handle correctly - fail for now
                Failed = true;
                return node;
            }

            public override SyntaxNode? VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
            {
                // Implicit array like: new[] { 1, 2, 3 } is complex - don't preserve
                Failed = true;
                return node;
            }

            public override SyntaxNode? VisitGenericName(GenericNameSyntax node)
            {
                var symbolInfo = _semanticModel.GetSymbolInfo(node);
                if (symbolInfo.Symbol is ITypeSymbol typeSymbol)
                {
                    var fqn = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    return SyntaxFactory.ParseTypeName(fqn).WithTriviaFrom(node);
                }
                return base.VisitGenericName(node);
            }

            public override SyntaxNode? VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
            {
                // Array creation with initializer is complex - don't preserve
                if (node.Initializer is not null)
                {
                    Failed = true;
                    return node;
                }
                return base.VisitArrayCreationExpression(node);
            }

            public override SyntaxNode? VisitTypeOfExpression(TypeOfExpressionSyntax node)
            {
                var typeInfo = _semanticModel.GetTypeInfo(node.Type);
                if (typeInfo.Type is not null)
                {
                    var fqn = typeInfo.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    return SyntaxFactory.TypeOfExpression(SyntaxFactory.ParseTypeName(fqn))
                        .WithTriviaFrom(node);
                }
                return base.VisitTypeOfExpression(node);
            }

            public override SyntaxNode? VisitDefaultExpression(DefaultExpressionSyntax node)
            {
                if (node.Type is not null)
                {
                    var typeInfo = _semanticModel.GetTypeInfo(node.Type);
                    if (typeInfo.Type is not null)
                    {
                        var fqn = typeInfo.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                        return SyntaxFactory.DefaultExpression(SyntaxFactory.ParseTypeName(fqn))
                            .WithTriviaFrom(node);
                    }
                }
                return base.VisitDefaultExpression(node);
            }

            public override SyntaxNode? VisitCastExpression(CastExpressionSyntax node)
            {
                var typeInfo = _semanticModel.GetTypeInfo(node.Type);
                if (typeInfo.Type is not null)
                {
                    var fqn = typeInfo.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    var newExpr = (ExpressionSyntax?)Visit(node.Expression) ?? node.Expression;
                    return SyntaxFactory.CastExpression(SyntaxFactory.ParseTypeName(fqn), newExpr)
                        .WithTriviaFrom(node);
                }
                return base.VisitCastExpression(node);
            }
        }
    }
}
