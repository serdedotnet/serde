using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serde;

internal partial class DeserializeImplGen
{
    /// <summary>
    /// Generates the method body for deserializing an enum.
    /// Code looks like:
    /// <code>
    /// static T IDeserialize&lt;T&gt;Deserialize(IDeserializer deserializer)
    /// {
    ///    var serdeInfo = GetInfo(this);
    ///    var index = deserializer.ReadEnum(serdeInfo);
    ///    return index switch
    ///    {
    ///      {index} =&gt; {enum member},
    ///      ...
    ///      _ =&gt; throw new InvalidDeserializeValueException($"Unexpected index: {index}")
    ///    };
    /// }
    /// </code>
    /// </summary>
    private static SourceBuilder GenerateEnumDeserializeMethod(
        GeneratorExecutionContext context,
        ITypeSymbol type,
        TypeSyntax typeSyntax
    )
    {
        Debug.Assert(type.TypeKind == TypeKind.Enum);
        var enumType = (INamedTypeSymbol)type;
        var typeFqn = typeSyntax.ToString();

        // When `AsUnderlying = true`, read the underlying integral value directly (e.g.
        // `(Color)deserializer.ReadI32()`) instead of reading a name.
        if (Proxies.EnumSerializesAsUnderlying(enumType))
        {
            var primName = Proxies.TryGetPrimitiveName(enumType.EnumUnderlyingType!)!;
            return new SourceBuilder(
                $$"""
{{typeFqn}} IDeserialize<{{typeFqn}}>.Deserialize(IDeserializer deserializer)
{
    return ({{typeFqn}})deserializer.Read{{primName}}();
}
"""
            );
        }

        var members = SymbolUtilities.GetDataMembers(type, SerdeUsage.Both, context);
        var src = new SourceBuilder(
            $$"""
{{typeFqn}} IDeserialize<{{typeFqn}}>.Deserialize(IDeserializer deserializer)
{
    var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
    var index = deserializer.ReadEnum(serdeInfo);
    {{typeFqn}} _l_result = index switch {
        {{string.Join("," + Utilities.NewLine, members
        .Select((m, i) => $"{i} => {typeSyntax}.{m.Name}"))}},
        _ => throw new InvalidOperationException($"Unexpected index: {index}")
    };
    return _l_result;
}
"""
        );
        return src;
    }
}
