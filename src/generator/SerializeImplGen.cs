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
    internal static SourceBuilder GenSerialize(
        GeneratorExecutionContext context,
        ITypeSymbol receiverType,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        if (receiverType.IsAbstract)
        {
            return GenUnionSerializeMethod((INamedTypeSymbol)receiverType);
        }

        var statements = new SourceBuilder();
        var fieldsAndProps = SymbolUtilities.GetDataMembers(receiverType, SerdeUsage.Serialize);

        // The generated body of ISerialize is
        // `var _l_info = GetInfo(this);
        // `var _l_type = serializer.WriteType(_l_info);
        // type.WriteValue<FieldType, Serialize>(_l_info, FieldIndex, receiver.FieldValue);
        // type.End();

        // If the type is an enum, we only want to serialize one field (the enum value), not all fields
        if (receiverType.TypeKind == TypeKind.Enum)
        {
            // `var _l_info = GetInfo(this);`
            statements.AppendLine($"var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);");
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
            // `var _l_info = GetInfo(this);`
            statements.AppendLine($"var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);");

            // `var type = serializer.WriteType(_l_info);`
            statements.AppendLine("var _l_type = serializer.WriteType(_l_info);");

            for (int i = 0; i < fieldsAndProps.Count; i++)
            {
                var m = fieldsAndProps[i];

                // 1. Check if this member has an explicit proxy. If so, we'll use it.
                if (Proxies.TryGetExplicitWrapper(m, context, SerdeUsage.Serialize, inProgress) is { } proxy)
                {
                    statements.AppendLine(MakeWriteValueStmt(m, m.Type.ToDisplayString(), proxy, i));
                }
                // 2. Check for a direct implementation of ISerialize
                else if (SerdeImplRoslynGenerator.ImplementsSerde(m.Type, m.Type, context, SerdeUsage.Serialize))
                {
                    statements.AppendLine(MakeWriteValueStmt(m, m.Type.ToDisplayString(), m.Type.ToDisplayString(), i));
                }
                // 3. Check if the member type is a primitive type. If so, it has a dedicated 'Write'
                //    method. Check using the non-null form (even if it's nullable), since nullable
                //    types aren't considered primitives
                else if (Proxies.TryGetPrimitiveName(m.Type.WithNullableAnnotation(NullableAnnotation.NotAnnotated)) is { } primName)
                {
                    if (m.IsNullable && !m.SerializeNull)
                    {
                        // Use WriteValueIfNotNull if it's not been disabled and the field is nullable
                        primName += "IfNotNull";
                    }
                    statements.AppendLine($"_l_type.Write{primName}(_l_info, {i}, value.{m.Name});");
                }
                // 4. A wrapper that implements ISerialize
                else if (Proxies.TryGetImplicitWrapper(m.Type, context, SerdeUsage.Serialize, inProgress) is { } wrapper)
                {
                    statements.AppendLine(MakeWriteValueStmt(m, wrapper.Type, wrapper.Proxy, i));
                }
                else
                {
                    // No built-in handling and doesn't implement ISerialize, error
                    context.ReportDiagnostic(CreateDiagnostic(
                        DiagId.ERR_DoesntImplementInterface,
                        m.Locations[0],
                        m.Symbol,
                        m.Type,
                        "Serde.ISerializeProvider<T>"));
                    continue;
                }
            }

            static string MakeWriteValueStmt(DataMemberSymbol m, string type, string proxy, int i)
            {
                // Generate statements of the form `type.WriteValue<FieldType, Serialize>("FieldName", value.FieldValue)`
                string methodName = m.Type.IsReferenceType
                    ? "WriteValue"
                    : "WriteBoxedValue";
                // Use WriteValueIfNotNull if it's not been disabled and the field is nullable
                if (m.IsNullable && !m.SerializeNull)
                {
                    methodName += "IfNotNull";
                }
                return $"_l_type.{methodName}<{type}, {proxy}>(_l_info, {i}, value.{m.Name});";
            }
        }
        // `type.End();`
        statements.Append("_l_type.End(_l_info);");

        var receiverSyntax = ((INamedTypeSymbol)receiverType).ToFqnSyntax();
        var receiverString = receiverType.ToDisplayString();

        // Generate method `void ISerialize<type>.Serialize(type value, ISerializer serializer) { ... }`
        var members = new SourceBuilder($$"""
        void global::Serde.ISerialize<{{receiverSyntax}}>.Serialize({{receiverSyntax}} value, global::Serde.ISerializer serializer)
        {
            {{statements}}
        }

        """);
        return members;
    }

    /// <summary>
    /// Generate the ISerialize{T}.Serialize method for a union type.
    /// </summary>
    private static SourceBuilder GenUnionSerializeMethod(INamedTypeSymbol baseType)
    {
        Debug.Assert(baseType.IsAbstract);

        // Unions are effectively a parent type and nested type. The parent type has a single
        // field, with the name being the type name and the value being the record case.

        var caseTypes = SymbolUtilities.GetDUTypeMembers(baseType);
        var casesBuilder = new SourceBuilder();
        for (int i = 0; i < caseTypes.Length; i++)
        {
            var t = caseTypes[i];
            var tString = t.ToDisplayString();
            casesBuilder.AppendLine($"case {tString} c:");
            casesBuilder.AppendLine($"    _l_type.WriteValue<{tString}, {SerdeInfoGenerator.GetUnionProxyName(t)}>(_l_serdeInfo, {i}, c);");
            casesBuilder.AppendLine($"    break;");
        }
        var methodDecl = new SourceBuilder($$"""
        void ISerialize<{{baseType.ToDisplayString()}}>.Serialize({{baseType.ToDisplayString()}} value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_serdeInfo);
            switch (value)
            {
                {{casesBuilder}}
            }
            _l_type.End(_l_serdeInfo);
        }
        """);
        return methodDecl;
    }
}