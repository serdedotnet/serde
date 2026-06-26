//HintName: ColorULongProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class ColorULongProxy : Serde.IDeserialize<ColorULong>
{
    ColorULong IDeserialize<ColorULong>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var de = deserializer.ReadType(serdeInfo);
        var (index, errorName) = de.TryReadIndexWithName(serdeInfo);
        if (index == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        ColorULong _l_result;
        if (index == ITypeDeserializer.EndOfType)
        {
            // Assume we want to read the underlying value
            _l_result = (ColorULong)de.ReadU64(serdeInfo, index);
        }
        else
        {
            _l_result = index switch {
                0 => ColorULong.Red,
                1 => ColorULong.Green,
                2 => ColorULong.Blue,
                _ => throw new InvalidOperationException($"Unexpected index: {index}")
            };
        }
        de.End(serdeInfo);
        return _l_result;
    }
}
