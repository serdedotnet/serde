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

        // The generated body of ISerialize is
        // `var _l_info = {TypeName}SerdeTypeInfo.TypeInfo;`
        // `var _l_type = serializer.WriteType(_l_info);
        // type.WriteField<FieldType, Serialize>(_l_info, FieldIndex, receiver.FieldValue);
        // type.End();

        // If the type is an enum, we only want to serialize one field (the enum value), not all fields
        if (receiverType.TypeKind == TypeKind.Enum)
        {
            // `var _l_info = GetInfo<{TypeName}Proxy>();`
            statements.AppendLine($"var _l_info = global::Serde.SerdeInfoProvider.GetInfo<{typeDeclContext.Name}Proxy>();");
            // `var _l_type = serializer.WriteType(_l_info);`
            statements.AppendLine("var _l_type = serializer.WriteType(_l_info);");

            // ```
            // var index = value switch
            // {
            //   Enum.Case1 => 0,
            //   Enum.Case2 => 1,
            //   var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum '{TypeName}'");
            // };
            // serializer.SerializeEnumValue("Enum", name, (Underlying)value, default(Underlying));
            var enumType = (INamedTypeSymbol)receiverType;
            var underlying = enumType.EnumUnderlyingType!;
            statements.AppendLine($$"""
                var index = value switch
                {
                    {{string.Join("," + Utilities.NewLine, fieldsAndProps
                        .Select((m, i) => $"{enumType.ToDisplayString()}.{m.Name} => {i}"))}},
                    var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum '{{enumType.Name}}'"),
                };
                """
            );
            var methodName = Proxies.TryGetPrimitiveName(underlying).NotNull();
            statements.AppendLine($"_l_type.Write{methodName}(_l_info, index, ({underlying.ToDisplayString()})value);");
        }
        else
        {
            // `var _l_info = {TypeName}SerdeTypeInfo.TypeInfo;`
            statements.AppendLine($"var _l_info = global::Serde.SerdeInfoProvider.GetInfo<{typeDeclContext.Name}{typeDeclContext.TypeParameterList}>();");

            // `var type = serializer.WriteType(_l_info);`
            statements.AppendLine("var _l_type = serializer.WriteType(_l_info);");

            for (int i = 0; i < fieldsAndProps.Count; i++)
            {
                var m = fieldsAndProps[i];

                TypeWithProxy typeAndProxy;
                // Check if this member has an explicit proxy. If so, we'll use it.
                if (Proxies.TryGetExplicitWrapper(m, context, SerdeUsage.Serialize, inProgress) is { } proxy)
                {
                    typeAndProxy = new(m.Type.ToFqnSyntax(), proxy);
                }
                // Check if the member type is a primitive type. If so, it has a dedicated 'Write' method
                else if (Proxies.TryGetPrimitiveName(m.Type) is { } methodName &&
                    m.Type.NullableAnnotation != NullableAnnotation.Annotated)
                {
                    statements.AppendLine($"_l_type.Write{methodName}(_l_info, {i}, value.{m.Name});");
                    continue;
                }

                // Generate statements of the form `type.WriteField<FieldType, Serialize>("FieldName", value.FieldValue)`
                var typeAndWrapperOpt = MakeSerializeType(m, context, inProgress);
                if (typeAndWrapperOpt is not { } typeAndWrapper)
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
                    string methodName = m.Type.IsReferenceType
                        ? "WriteField"
                        : "WriteBoxedField";
                    // Use WriteFieldIfNotNull if it's not been disabled and the field is nullable
                    if (m.IsNullable && !m.SerializeNull)
                    {
                        methodName += "IfNotNull";
                    }
                    statements.AppendLine($"_l_type.{methodName}<{typeAndWrapper.Type}, {typeAndWrapper.Proxy}>(_l_info, {i}, value.{m.Name});");
                }
            }
        }
        // `type.End();`
        statements.Append("_l_type.End(_l_info);");

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
            casesBuilder.AppendLine($"    _l_type.WriteField<{tString}, {SerdeInfoGenerator.GetUnionProxyName(t)}>(_l_serdeInfo, {i}, c);");
            casesBuilder.AppendLine($"    break;");
        }
        string methodDecl = $$"""
        void ISerialize<{{baseType.ToDisplayString()}}>.Serialize({{baseType.ToDisplayString()}} value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<{{baseType.ToDisplayString()}}>();
            var _l_type = serializer.WriteType(_l_serdeInfo);
            switch (value)
            {
                {{casesBuilder}}
            }
            _l_type.End(_l_serdeInfo);
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
}