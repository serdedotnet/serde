using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Serde.Diagnostics;

namespace Serde;

public partial class SerializeImplGen
{
    internal static SourceBuilder GenSerialize(
        GeneratorExecutionContext context,
        ITypeSymbol receiverType,
        INamedTypeSymbol? foreignType,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress
    )
    {
        if (receiverType.IsAbstract)
        {
            return GenUnionSerializeMethod((INamedTypeSymbol)receiverType);
        }

        var statements = new SourceBuilder();
        var fieldsAndProps = SymbolUtilities.GetDataMembers(
            receiverType,
            SerdeUsage.Serialize,
            context
        );

        // The generated body of ISerialize is
        // `var _l_info = GetInfo(this);
        // `var _l_type = serializer.WriteType(_l_info);
        // type.WriteValue<FieldType, Serialize>(_l_info, FieldIndex, receiver.FieldValue);
        // type.End();

        // If the type is an enum, we only want to serialize one field (the enum value), not all fields
        if (receiverType.TypeKind == TypeKind.Enum)
        {
            GenEnumSerialize(receiverType, statements, fieldsAndProps);
        }
        else
        {
            var classScopeProxyMap = ProxyMap.FromSymbol(receiverType);

            // When serializing for a foreign type, convert the foreign value to the
            // proxy via the explicit conversion operator and read members off the
            // proxy. Otherwise read members directly off the value.
            string receiverExpr;
            if (foreignType is not null)
            {
                receiverExpr = "_l_self";
                statements.AppendLine($"var _l_self = ({receiverType.ToDisplayString()})value;");
            }
            else
            {
                receiverExpr = "value";
            }

            // `var _l_info = GetInfo(this);`
            statements.AppendLine($"var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);");

            // The field-writing statements are collected separately so that the number of fields
            // that will actually be written can be computed and passed to WriteType before any of
            // them are emitted. Nullable fields that are skipped when null (i.e. serialized with an
            // `IfNotNull` helper) are subtracted from the count at runtime.
            var writeStatements = new SourceBuilder();

            // Member-access expressions for fields that may be skipped when null. Each contributes a
            // `-1` to the field count when its value is null.
            var skippableExprs = new List<string>();

            for (int i = 0; i < fieldsAndProps.Count; i++)
            {
                var m = fieldsAndProps[i];

                var proxyContext = ProxyContext.Create(
                    classScopeProxyMap,
                    ProxyMap.FromSymbol(m.Symbol)
                );

                var notNullTypeName = (
                    m.Type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T
                        ? ((INamedTypeSymbol)m.Type).TypeArguments[0]
                        : m.Type.WithNullableAnnotation(NullableAnnotation.NotAnnotated)
                ).ToDisplayString();

                // 1. Check if this member has an explicit proxy. If so, we'll use it.
                string writeStmt;
                if (
                    Proxies.TryGetExplicitWrapper(
                        m,
                        context,
                        SerdeUsage.Serialize,
                        inProgress,
                        proxyContext
                    ) is
                    { } proxy
                )
                {
                    writeStmt = MakeWriteValueStmt(m, notNullTypeName, proxy, i, receiverExpr);
                }
                // 2. Check for a direct implementation of ISerialize
                else if (
                    SerdeImplRoslynGenerator.ImplementsSerde(
                        m.Type,
                        m.Type,
                        context,
                        SerdeUsage.Serialize
                    )
                )
                {
                    writeStmt = MakeWriteValueStmt(m, notNullTypeName, notNullTypeName, i, receiverExpr);
                }
                // 3. Check if the member type is a primitive type. If so, it has a dedicated 'Write'
                //    method. Check using the non-null form (even if it's nullable), since nullable
                //    types aren't considered primitives
                else if (
                    Proxies.TryGetPrimitiveName(
                        m.Type.WithNullableAnnotation(NullableAnnotation.NotAnnotated)
                    ) is
                    { } primName
                )
                {
                    if (m.IsNullable && !m.SerializeNull)
                    {
                        // Use WriteValueIfNotNull if it's not been disabled and the field is nullable
                        primName += "IfNotNull";
                    }
                    writeStmt = $"_l_type.Write{primName}(_l_info, {i}, {receiverExpr}.{m.Name});";
                }
                // 4. A wrapper that implements ISerialize
                else if (
                    Proxies.TryGetImplicitWrapper(
                        m.Type,
                        context,
                        SerdeUsage.Serialize,
                        inProgress,
                        proxyContext
                    ) is
                    { } wrapper
                )
                {
                    writeStmt = MakeWriteValueStmt(m, notNullTypeName, wrapper.Proxy, i, receiverExpr);
                }
                else
                {
                    // No built-in handling and doesn't implement ISerialize, error
                    context.ReportDiagnostic(
                        CreateDiagnostic(
                            DiagId.ERR_DoesntImplementInterface,
                            m.Locations[0],
                            m.Symbol,
                            m.Type,
                            "Serde.ISerializeProvider<T>"
                        )
                    );
                    continue;
                }

                writeStatements.AppendLine(writeStmt);

                // A nullable field that is not forced to serialize null is skipped when its value is
                // null, so it must be removed from the field count in that case.
                if (m.IsNullable && !m.SerializeNull)
                {
                    skippableExprs.Add($"{receiverExpr}.{m.Name}");
                }

                static string MakeWriteValueStmt(
                    DataMemberSymbol m,
                    string type,
                    string proxy,
                    int i,
                    string valueExpr
                )
                {
                    // Generate statements of the form `type.WriteValue<FieldType, Serialize>("FieldName", value.FieldValue)`
                    // Use WriteValueIfNotNull if it's not been disabled and the field is nullable. In that
                    // case the generic argument is the non-null inner type, because the IfNotNull overloads
                    // accept a `T?` value and a provider of the nullable type.
                    if (m.IsNullable && !m.SerializeNull)
                    {
                        return $"_l_type.WriteValueIfNotNull<{type}, {proxy}>(_l_info, {i}, {valueExpr}.{m.Name});";
                    }
                    // Plain WriteValue: the generic type argument must match the static type of the value
                    // expression and the provider. For a nullable member (e.g. `int?` serialized via
                    // NullableProxy when SerializeNull is set) that is the full nullable type, not the
                    // unwrapped inner type.
                    var typeArg = m.IsNullable ? m.Type.ToDisplayString() : type;
                    return $"_l_type.WriteValue<{typeArg}, {proxy}>(_l_info, {i}, {valueExpr}.{m.Name});";
                }
            }

            // Compute the number of fields that will actually be written and open the type. When no
            // fields can be skipped the count is a constant; otherwise it starts at the total and is
            // decremented for each nullable field whose value is null.
            if (skippableExprs.Count == 0)
            {
                statements.AppendLine(
                    $"var _l_type = serializer.WriteType(_l_info, {fieldsAndProps.Count});"
                );
            }
            else
            {
                statements.AppendLine($"var _l_fieldCount = {fieldsAndProps.Count};");
                foreach (var expr in skippableExprs)
                {
                    statements.AppendLine($"if ({expr} is null) _l_fieldCount--;");
                }
                statements.AppendLine("var _l_type = serializer.WriteType(_l_info, _l_fieldCount);");
            }

            statements.Append(writeStatements);

            // `type.End();`
            statements.Append("_l_type.End(_l_info);");
        }

        // The interface type is the foreign type when present, otherwise the receiver.
        var interfaceType = (ITypeSymbol?)foreignType ?? receiverType;
        var interfaceString = interfaceType.ToDisplayString();

        // Generate method `void ISerialize<type>.Serialize(type value, ISerializer serializer) { ... }`
        var members = new SourceBuilder(
            $$"""
            void global::Serde.ISerialize<{{interfaceString}}>.Serialize({{interfaceString}} value, global::Serde.ISerializer serializer)
            {
                {{statements}}
            }

            """
        );
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
            casesBuilder.AppendLine(
                $"    _l_type.WriteValue<{tString}, {SerdeInfoGenerator.GetUnionProxyName(t)}>(_l_serdeInfo, {i}, c);"
            );
            casesBuilder.AppendLine($"    break;");
        }
        var methodDecl = new SourceBuilder(
            $$"""
            void ISerialize<{{baseType.ToDisplayString()}}>.Serialize({{baseType.ToDisplayString()}} value, ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_serdeInfo, 1);
                switch (value)
                {
                    {{casesBuilder}}
                }
                _l_type.End(_l_serdeInfo);
            }
            """
        );
        return methodDecl;
    }
}
