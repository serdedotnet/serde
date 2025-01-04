using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;

namespace Serde;

public partial class SerializeImplGen
{
    internal static (SourceBuilder, string BaseList) GenSerialize(
        TypeDeclContext typeDeclContext,
        GeneratorExecutionContext context,
        ITypeSymbol receiverType,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        if (receiverType.IsAbstract)
        {
            return ( new(GenUnionSerializeMethod((INamedTypeSymbol)receiverType)),
                     $": Serde.ISerialize<{receiverType.ToDisplayString()}>");
        }

        var statements = new SourceBuilder();
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
            statements.AppendLine($"var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<{typeDeclContext.Name}Proxy>();");
            statements.AppendLine($$"""
                var index = value switch
                {
                    {{string.Join("," + Utilities.NewLine, fieldsAndProps
                        .Select((m, i) => $"{typeSyntax}.{m.Name} => {i}")) }},
                    var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum '{{enumType.Name}}'"),
                };
                """
            );
            var wrapper = Proxies.TryGetPrimitiveProxy(underlying, SerdeUsage.Serialize).NotNull().Proxy;
            statements.AppendLine(
                $"serializer.SerializeEnumValue(_l_serdeInfo, index, ({underlying.ToFqnSyntax()})value, {wrapper.ToFullString()}.Instance);"
            );
        }
        else
        {
            // The generated body of ISerialize is
            // `var _l_serdeInfo = {TypeName}SerdeTypeInfo.TypeInfo;`
            // `var type = serializer.SerializeType(_l_serdeInfo);
            // type.SerializeField<FieldType, Serialize>(_l_serdeInfo, FieldIndex, receiver.FieldValue);
            // type.End();

            // `var _l_serdeInfo = {TypeName}SerdeTypeInfo.TypeInfo;`
            statements.AppendLine($"var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<{typeDeclContext.Name}{typeDeclContext.TypeParameterList}>();");

            // `var type = serializer.SerializeType(_l_serdeInfo);`
            statements.AppendLine("var type = serializer.SerializeType(_l_serdeInfo);");

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
                    statements.AppendLine(MakeSerializeFieldStmt(m, i, memberExpr, typeAndWrapper, IdentifierName("value")).ToFullString());
                }
            }

            // `type.End();`
            statements.Append("type.End();");
        }

        var receiverSyntax = ((INamedTypeSymbol)receiverType).ToFqnSyntax();
        var receiverString = receiverType.ToDisplayString();

        string serdeInfoText = $"""
static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider<{receiverSyntax}>.SerdeInfo => {receiverSyntax}SerdeInfo.Instance;
""";
        // Generate method `void ISerialize<type>.Serialize(type value, ISerializer serializer) { ... }`
        var members = new SourceBuilder($$"""
        void global::Serde.ISerialize<{{receiverSyntax}}>.Serialize({{receiverSyntax}} value, global::Serde.ISerializer serializer)
        {
            {{statements}}
        }

        """);
        List<BaseTypeSyntax> bases = [
            SimpleBaseType(ParseTypeName($"Serde.ISerialize<{receiverString}>")),
        ];
        if (receiverType.TypeKind == TypeKind.Enum)
        {
            bases.Add(SimpleBaseType(ParseTypeName($"Serde.ISerializeProvider<{receiverType.ToDisplayString()}>")));
            members.AppendLine($$"""
            static ISerialize<{{receiverString}}> ISerializeProvider<{{receiverString}}>.SerializeInstance
                => {{receiverString}}Proxy.Instance;
            """);
        }
        return (members, BaseList(SeparatedList(bases)).ToFullString());

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
    /// Generate the ISerialize{T}.Serialize method for a union type.
    /// </summary>
    private static string GenUnionSerializeMethod(INamedTypeSymbol baseType)
    {
        Debug.Assert(baseType.IsAbstract);

        // Unions are effectively a parent type and nested type. The parent type has a single
        // field, with the name being the type name and the value being the record case.

        var caseTypes = SymbolUtilities.GetDUTypeMembers(baseType);
        var casesBuilder = new StringBuilder();
        for (int i = 0; i < caseTypes.Length; i++)
        {
            var t = caseTypes[i];
            var tString = t.ToDisplayString();
            casesBuilder.AppendLine($"case {tString} c:");
            casesBuilder.AppendLine($"    _l_type.SerializeField<{tString}, {SerdeInfoGenerator.GetUnionProxyName(t)}>(_l_serdeInfo, {i}, c);");
            casesBuilder.AppendLine($"    break;");
        }
        string methodDecl = $$"""
        void ISerialize<{{baseType.ToDisplayString()}}>.Serialize({{baseType.ToDisplayString()}} value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<{{baseType.ToDisplayString()}}>();
            var _l_type = serializer.SerializeType(_l_serdeInfo);
            switch (value)
            {
                {{casesBuilder}}
            }
            _l_type.End();
        }
        """;
        return methodDecl;
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