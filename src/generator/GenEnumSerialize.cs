using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Serde;

public partial class SerializeImplGen
{
    /// <summary>
    /// Generates the serialization code for an enum type. The generated code looks like:
    /// <code>
    /// void ISerialize&lt;T&gt;.Serialize(T value, ISerializer serializer)
    /// {
    ///    var _l_info = GetInfo(this);
    ///    var _l_index = value switch
    ///    {
    ///        Enum.Case1 => 0,
    ///        Enum.Case2 => 1,
    ///        var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum '{TypeName}'");
    ///    };
    ///    serializer.WriteEnum(_l_info, _l_index);
    /// }
    /// </code>
    /// </summary>
    private static void GenEnumSerialize(
        ITypeSymbol receiverType,
        SourceBuilder statements,
        List<DataMemberSymbol> fieldsAndProps
    )
    {
        var enumType = (INamedTypeSymbol)receiverType;
        var underlying = enumType.EnumUnderlyingType!;

        // When `AsUnderlying = true`, serialize the enum directly as its underlying integral value
        // (e.g. `serializer.WriteI32((int)value);`) instead of by name.
        if (Proxies.EnumSerializesAsUnderlying(enumType))
        {
            var primName = Proxies.TryGetPrimitiveName(underlying)!;
            statements.AppendLine(
                $"serializer.Write{primName}(({underlying.ToDisplayString()})value);"
            );
            return;
        }

        // `var _l_info = GetInfo(this);`
        statements.AppendLine($"var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);");

        // ```
        // var _l_index = value switch
        // {
        //   Enum.Case1 => 0,
        //   Enum.Case2 => 1,
        //   var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum '{TypeName}'");
        // };
        statements.AppendLine(
            $$"""
                var _l_index = value switch
                {
                    {{string.Join("," + Utilities.NewLine, fieldsAndProps
                    .Select((m, i) => $"{enumType.ToDisplayString()}.{m.Name} => {i}"))}},
                    var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum '{{enumType.Name}}'"),
                };
                """
        );

        // `var _l_type = serializer.WriteEnum(_l_info, );`
        statements.AppendLine("serializer.WriteEnum(_l_info, _l_index);");
    }
}
