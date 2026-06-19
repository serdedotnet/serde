//HintName: ColorProxy.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial class ColorProxy : Serde.ISerde<Color>
{
    void global::Serde.ISerialize<Color>.Serialize(Color value, global::Serde.ISerializer serializer)
    {
        var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
        var _l_type = serializer.WriteType(_l_info);
        var index = value switch
        {
            Color.Red => 0,
            Color.Green => 1,
            Color.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'Color'"),
        };
        _l_type.WriteI32(_l_info, index, (int)value);
        _l_type.End(_l_info);
    }
    Color IDeserialize<Color>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        using var de = deserializer.ReadType(serdeInfo);
        var (index, errorName) = de.TryReadIndexWithName(serdeInfo);
        if (index == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        if (index == ITypeDeserializer.EndOfType)
        {
            // Assume we want to read the underlying value
            return (Color)de.ReadI32(serdeInfo, index);
        }
        return index switch {
            0 => Color.Red,
            1 => Color.Green,
            2 => Color.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
}
