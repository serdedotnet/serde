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

namespace Serde
{
    public partial class SerializeImplRoslynGenerator
    {
        internal static (MemberDeclarationSyntax[], BaseListSyntax) GenerateSerializeGenericImpl(
            GeneratorExecutionContext context,
            ITypeSymbol receiverType,
            ImmutableList<ITypeSymbol> inProgress)
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
                statements.Add(ParseStatement($"var _l_typeInfo = {receiverType.ToFqnSyntax()}SerdeTypeInfo.TypeInfo;"));
                statements.Add(ParseStatement($$"""
                var index = value switch
                {
                    {{string.Join("," + Environment.NewLine, fieldsAndProps
                        .Select((m, i) => $"{typeSyntax}.{m.Name} => {i}")) }},
                    var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum '{{enumType.Name}}'"),
                };
                """));
                var wrapper = TryGetPrimitiveWrapper(underlying, SerdeUsage.Serialize).Unwrap().Wrapper;
                statements.Add(ParseStatement(
                    $"serializer.SerializeEnumValue(_l_typeInfo, index, ({underlying.ToFqnSyntax()})value, default({wrapper.ToFullString()}));"));
            }
            else
            {
                // The generated body of ISerialize is
                // `var _l_typeInfo = {TypeName}SerdeTypeInfo.TypeInfo;`
                // `var type = serializer.SerializeType(_l_typeInfo);
                // type.SerializeField<FieldType, Serialize>(_l_typeInfo, FieldIndex, receiver.FieldValue);
                // type.End();

                // `var _l_typeInfo = {TypeName}SerdeTypeInfo.TypeInfo;`
                statements.Add(ParseStatement($"var _l_typeInfo = {receiverType.Name}SerdeTypeInfo.TypeInfo;"));

                // `var type = serializer.SerializeType(_l_typeInfo);`
                statements.Add(ParseStatement("var type = serializer.SerializeType(_l_typeInfo);"));

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
                            "Serde.ISerialize"));
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

            // Generate method `void ISerialize<type>.Serialize(type value, ISerializer serializer) { ... }`
            var members = new MemberDeclarationSyntax[] {
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
            };
            var baseList = BaseList(SeparatedList(new BaseTypeSyntax[] {
                    SimpleBaseType(ParseTypeName($"Serde.ISerialize<{receiverType.ToDisplayString()}>"))
                }));
            return (members, baseList);

            // Make a statement like `type.SerializeField<valueType, SerializeType>("member.Name", value)`
            static ExpressionStatementSyntax MakeSerializeFieldStmt(
                DataMemberSymbol member,
                int index,
                ExpressionSyntax value,
                TypeAndWrapper typeAndWrapper,
                ExpressionSyntax receiver)
            {
                var arguments = new List<ExpressionSyntax>() {
                        // _l_typeInfo
                        ParseExpression("_l_typeInfo"),
                        // Index
                        LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(index)),
                        // Value
                        value,
                };
                var typeArgs = new List<TypeSyntax>() {
                    typeAndWrapper.Type,
                    typeAndWrapper.Wrapper
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
        private static TypeAndWrapper? MakeSerializeType(
            DataMemberSymbol member,
            GeneratorExecutionContext context,
            ImmutableList<ITypeSymbol> inProgress)
        {
            // 1. Check for an explicit wrapper
            if (TryGetExplicitWrapper(member, context, SerdeUsage.Serialize, inProgress) is {} wrapper)
            {
                return new(member.Type.ToFqnSyntax(), wrapper);
            }

            // 2. Check for a direct implementation of ISerialize
            if (SerdeImplRoslynGenerator.ImplementsSerde(member.Type, member.Type, context, SerdeUsage.Serialize))
            {
                return new(member.Type.ToFqnSyntax(),
                    GenericName(Identifier("IdWrap"), TypeArgumentList(SeparatedList(new[] { member.Type.ToFqnSyntax() }))));
            }

            // 3. A wrapper that implements ISerialize
            return TryGetAnyWrapper(member.Type, context, SerdeUsage.Serialize, inProgress);
        }

        private static TypeSyntax? TryGetExplicitWrapper(
            DataMemberSymbol member,
            GeneratorExecutionContext context,
            SerdeUsage usage,
            ImmutableList<ITypeSymbol> inProgress)
        {
            if (TryGetExplicitWrapperType(member, usage, context) is {} wrapperType)
            {
                var memberType = member.Type;
                if (SerdeImplRoslynGenerator.ImplementsSerde(wrapperType, memberType, context, usage))
                {
                    // If the wrapper type directly implements the interface, just return it
                    return ParseTypeName(wrapperType.ToDisplayString());
                }

                // Otherwise, the wrapper type should be an unconstructed generic type which we need to
                // construct with the appropriate wrappers for the member type's type arguments.
                var typeArgs = memberType switch
                {
                    INamedTypeSymbol n => n.TypeArguments,
                    _ => ImmutableArray<ITypeSymbol>.Empty
                };
                return MakeWrappedExpression(wrapperType, typeArgs, context, usage, inProgress);
            }
            return null;

            // <summary>
            // Looks to see if the given member or its type explicitly specifies a wrapper to use via
            // the SerdeWrapAttribute or similar. If so, returns the symbol of the wrapper type.
            // </summary>
            static INamedTypeSymbol? TryGetExplicitWrapperType(DataMemberSymbol member, SerdeUsage usage, GeneratorExecutionContext context)
            {
                // Look first for a wrapper attribute on the member being serialized, and then for a
                // wrapper attribute
                var typeToWrap = member.Type;
                return GetSerdeWrapAttributeArg(member.Symbol, typeToWrap, usage, context)
                    ?? GetSerdeWrapAttributeArg(member.Type, typeToWrap, usage, context);

                static INamedTypeSymbol? GetSerdeWrapAttributeArg(ISymbol symbol, ITypeSymbol typeToWrap, SerdeUsage usage, GeneratorExecutionContext context)
                {
                    foreach (var attr in symbol.GetAttributes())
                    {
                        if (attr.AttributeClass?.Name is "SerdeWrapAttribute")
                        {
                            if (attr is { ConstructorArguments: { Length: 1 } attrArgs } &&
                                attrArgs[0] is { Value: INamedTypeSymbol wrapperType })
                            {
                                // If the typeToWrap is a generic type, we should expect that the wrapper type
                                // is not listed directly, but instead a parent type is listed (possibly static) and
                                // the Serialize and Deserialize wrappers are nested below.
                                if (typeToWrap.OriginalDefinition is INamedTypeSymbol { Arity: > 0 } wrapped)
                                {
                                    var nestedName = usage == SerdeUsage.Serialize ? "SerializeImpl" : "DeserializeImpl";
                                    var nestedTypes = wrapperType.GetTypeMembers(nestedName);
                                    if (nestedTypes.Length != 1)
                                    {
                                        context.ReportDiagnostic(CreateDiagnostic(
                                            DiagId.ERR_CantFindNestedWrapper,
                                            symbol.Locations[0],
                                            nestedName,
                                            wrapperType,
                                            wrapped));
                                        return null;
                                    }
                                    return nestedTypes[0];
                                }
                                return wrapperType;
                            }
                            // Return null if the attribute is somehow incorrect
                            // TODO: produce a warning?
                            return null;
                        }
                        else if (attr is { AttributeClass.Name: nameof(SerdeMemberOptions), NamedArguments: { } named })
                        {
                            foreach (var arg in named)
                            {
                                if (usage.HasFlag(SerdeUsage.Serialize)
                                    && arg is { Key: nameof(SerdeMemberOptions.WrapperSerialize),
                                                Value.Value: INamedTypeSymbol wrapperType })
                                {
                                    return wrapperType;
                                }
                                if (usage.HasFlag(SerdeUsage.Deserialize)
                                    && arg is { Key: nameof(SerdeMemberOptions.WrapperDeserialize),
                                                Value.Value: INamedTypeSymbol wrapperType2 })
                                {
                                    return wrapperType2;
                                }
                            }
                        }
                    }
                    return null;
                }
            }
        }

        private static TypeSyntax? MakeWrappedExpression(
            INamedTypeSymbol baseWrapper,
            ImmutableArray<ITypeSymbol> elemTypes,
            GeneratorExecutionContext context,
            SerdeUsage usage,
            ImmutableList<ITypeSymbol> inProgress)
        {
            if (elemTypes.Length == 0)
            {
                return baseWrapper.ToFqnSyntax();
            }

            var wrapperTypes = new List<TypeSyntax>();
            foreach (var elemType in elemTypes)
            {
                var elemSyntax = elemType.ToFqnSyntax();
                if (SerdeImplRoslynGenerator.ImplementsSerde(elemType, elemType, context, usage))
                {
                    // Special case for List-like types:
                    // If the element type directly implements ISerialize, we can
                    // use a single-arity version of the wrapper
                    //      ArrayWrap<`elemType`>
                    wrapperTypes.Add(elemSyntax);

                    // Otherwise we need an `IdWrap` which just delegates to the inner
                    // type.
                    //if (elemTypes.Length > 1)
                    if (usage == SerdeUsage.Serialize)
                    {
                        var idWrap = GenericName(Identifier("IdWrap"), TypeArgumentList(SeparatedList(new[] { elemSyntax })));
                        wrapperTypes.Add(idWrap);
                    }
                    else
                    {
                        wrapperTypes.Add(elemSyntax);
                    }
                    continue;
                }

                // Otherwise we'll need to wrap the element type as well e.g.,
                //      ArrayWrap<`elemType`, `elemTypeWrapper`>
                var typeAndWrapper = TryGetAnyWrapper(elemType, context, usage, inProgress);

                if (typeAndWrapper is not (_, var wrapper))
                {
                    // Could not find a wrapper
                    return null;
                }
                else
                {
                    wrapperTypes.Add(elemSyntax);
                    wrapperTypes.Add(wrapper);
                }
            }

            var wrapperSyntax = (QualifiedNameSyntax)baseWrapper.ToFqnSyntax();
            wrapperSyntax = wrapperSyntax.WithRight(
                ((GenericNameSyntax)wrapperSyntax.Right).WithTypeArgumentList(
                    TypeArgumentList(SeparatedList(wrapperTypes.ToArray()))));

            return wrapperSyntax;
        }

        private static TypeAndWrapper? TryGetAnyWrapper(
            ITypeSymbol elemType,
            GeneratorExecutionContext context,
            SerdeUsage usage,
            ImmutableList<ITypeSymbol> inProgress)
        {
            // If we're in the process of generating a wrapper type, just return the name of the wrapper
            // and assume it has been generated already.
            if (inProgress.Contains(elemType, SymbolEqualityComparer.Default))
            {
                var typeName = elemType.Name;
                var allTypes = typeName;
                for (var parent = elemType.ContainingType; parent is not null; parent = parent.ContainingType)
                {
                    allTypes = parent.Name + allTypes;
                }
                var wrapperName = $"{allTypes}Wrap";
                return new(elemType.ToFqnSyntax(), IdentifierName(wrapperName));
            }
            var typeAndWrapper = TryGetPrimitiveWrapper(elemType, usage)
                ?? TryGetEnumWrapper(elemType, usage)
                ?? TryGetCompoundWrapper(elemType, context, usage, inProgress);
            if (typeAndWrapper is null)
            {
                return null;
            }
            return typeAndWrapper;
        }

        // If the target is a core type, we can wrap it
        private static TypeAndWrapper? TryGetPrimitiveWrapper(ITypeSymbol type, SerdeUsage usage)
        {
            var idWrapper = SerdeImplRoslynGenerator.TryGetPrimitiveWrapper(type, usage);
            return idWrapper is null ? null : new(type.ToFqnSyntax(), idWrapper);
        }

        private static TypeAndWrapper? TryGetEnumWrapper(ITypeSymbol type, SerdeUsage usage)
        {
            if (type.TypeKind is not TypeKind.Enum)
            {
                return null;
            }

            // Check for the generation attributes
            if (!HasGenerateAttribute(type, usage))
            {
                return null;
            }

            var wrapperName = SerdeImplRoslynGenerator.GetWrapperName(type.Name);
            var containing = type.ContainingType?.ToDisplayString();
            if (containing is null && type.ContainingNamespace is { IsGlobalNamespace: false } ns)
            {
                containing = ns.ToDisplayString();
            }
            var wrapperFqn = containing is not null
                 ? containing + "." + wrapperName
                 : "global::" + wrapperName;

            return new(type.ToFqnSyntax(), ParseTypeName(wrapperFqn));
        }

        private static bool HasGenerateAttribute(ITypeSymbol memberType, SerdeUsage usage)
        {
            var attributes = memberType.GetAttributes();
            foreach (var attr in attributes)
            {
                var attrClass = attr.AttributeClass;
                if (attrClass is null)
                {
                    continue;
                }
                if (WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.GenerateSerde))
                {
                    return true;
                }
                if (usage == SerdeUsage.Serialize &&
                    WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.GenerateSerialize))
                {
                    return true;
                }
                if (usage == SerdeUsage.Deserialize &&
                    WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.GenerateDeserialize))
                {
                    return true;
                }
            }
            return false;
        }

        private static TypeAndWrapper? TryGetCompoundWrapper(ITypeSymbol type, GeneratorExecutionContext context, SerdeUsage usage, ImmutableList<ITypeSymbol> inProgress)
        {
            (TypeSyntax?, TypeSyntax?)? valueTypeAndWrapper = type switch
            {
                { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } =>
                    (null,
                     MakeWrappedExpression(
                        context.Compilation.GetTypeByMetadataName("Serde.NullableWrap+" + GetImplName(usage) + "`2")!,
                        ImmutableArray.Create(((INamedTypeSymbol)type).TypeArguments[0]),
                        context,
                        usage,
                        inProgress)),

                // This is rather subtle. One might think that we would want to use a
                // NullableRefWrapper for any reference type that could contain null. In fact, we
                // only want to use one if the type in question is actually annotated as nullable.
                // The difference comes down to type parameters. If a type parameter is constrained
                // as `class?` then it is both a reference type and nullable, but we don't want to
                // use a wrapper for it. The reason why is that in we don't know the actual
                // "underlying" type and couldn't dispatch to the underlying type's ISerialize
                // implementation. Instead, for type parameters that aren't actually "annotated" as
                // nullable (i.e., "T?") we must rely on the type parameter itself implementing
                // ISerialize, and therefore the substitution to provide the appropriate nullable
                // wrapper.
                { IsReferenceType: true, NullableAnnotation: NullableAnnotation.Annotated} =>
                    (null,
                     MakeWrappedExpression(
                        context.Compilation.GetTypeByMetadataName("Serde.NullableRefWrap+" + GetImplName(usage) + "`2")!,
                        ImmutableArray.Create(type.WithNullableAnnotation(NullableAnnotation.NotAnnotated)),
                        context,
                        usage,
                        inProgress)),

                IArrayTypeSymbol and { IsSZArray: true, Rank: 1, ElementType: { } elemType }
                    => (null,
                        MakeWrappedExpression(
                        context.Compilation.GetTypeByMetadataName("Serde.ArrayWrap+" + GetImplName(usage) + "`2")!,
                        ImmutableArray.Create(elemType),
                        context,
                        usage,
                        inProgress)),

                INamedTypeSymbol t when TryGetWrapperName(t, context, usage) is (var ValueType, (var WrapperType, var Args))
                    => (ValueType,
                        MakeWrappedExpression(WrapperType, Args, context, usage, inProgress)),

                _ => null,
            };
            return valueTypeAndWrapper switch {
                null => null,
                (null, {} wrapper) => new(type.ToFqnSyntax(), wrapper),
                ({ } value, { } wrapper) => new(value, wrapper),
                (_, null) => throw ExceptionUtilities.Unreachable
            };
        }

        private static string GetImplName(SerdeUsage usage) => usage switch
        {
            SerdeUsage.Serialize => "SerializeImpl",
            SerdeUsage.Deserialize => "DeserializeImpl",
            _ => throw ExceptionUtilities.Unreachable
        };

        private static (TypeSyntax MemberType, (INamedTypeSymbol WrapperType, ImmutableArray<ITypeSymbol> Args))? TryGetWrapperName(
            ITypeSymbol typeSymbol,
            GeneratorExecutionContext context,
            SerdeUsage usage)
        {
            if (typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
            {
                var nullableRefWrap = context.Compilation.GetTypeByMetadataName("Serde.NullableRefWrap+" + GetImplName(usage) + "`1")!;
                return (typeSymbol.ToFqnSyntax(),
                    (nullableRefWrap, ImmutableArray.Create(typeSymbol.WithNullableAnnotation(NullableAnnotation.NotAnnotated))));
            }
            if (typeSymbol is INamedTypeSymbol named && TryGetWellKnownType(named, context) is {} wk)
            {
                return (typeSymbol.ToFqnSyntax(), (ToWrapper(wk, context.Compilation, usage), named.TypeArguments));
            }

            // Check if it implements well-known interfaces
            foreach (var iface in WellKnownTypes.GetAvailableInterfacesInOrder(context))
            {
                Debug.Assert(iface.TypeKind == TypeKind.Interface);
                foreach (var impl in typeSymbol.Interfaces)
                {
                    if (impl.OriginalDefinition.Equals(iface, SymbolEqualityComparer.Default) &&
                        ToWrapper(TryGetWellKnownType(iface, context), context.Compilation, usage) is { } wrap)
                    {
                        return (impl.ToFqnSyntax(), (wrap, impl.TypeArguments));
                    }
                }
            }
            return null;
        }

        private readonly record struct TypeAndWrapper(TypeSyntax Type, TypeSyntax Wrapper);

        [return: NotNullIfNotNull(nameof(wk))]
        internal static INamedTypeSymbol? ToWrapper(WellKnownType? wk, Compilation comp, SerdeUsage usage)
        {
            if (wk is null)
            {
                return null;
            }
            var (baseName, arity) = wk.GetValueOrDefault() switch
            {
                WellKnownType.ImmutableArray_1 => ("ImmutableArrayWrap", 2),
                WellKnownType.List_1 => ("ListWrap", 2),
                WellKnownType.Dictionary_2 => ("DictWrap", 4),
                WellKnownType.IDictionary_2 => ("IDictWrap", 4),
                WellKnownType.IReadOnlyDictionary_2 => ("IRODictWrap", 4),
            };
            string fqn = "Serde." + baseName + "+" + GetImplName(usage) + "`" + arity;
            var type = comp.GetTypeByMetadataName(fqn)!;
            return type;
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

        private static LiteralExpressionSyntax NumericLiteral(int num)
            => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(num));

        private static LiteralExpressionSyntax StringLiteral(string text)
            => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(text));

        private static MemberAccessExpressionSyntax MakeMemberAccessExpr(DataMemberSymbol m, ExpressionSyntax receiverExpr)
            => MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                receiverExpr,
                IdentifierName(m.Name));
    }
}