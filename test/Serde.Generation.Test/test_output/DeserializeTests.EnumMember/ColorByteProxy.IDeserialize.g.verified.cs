//HintName: ColorByteProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class ColorByteProxy : Serde.IDeserialize<ColorByte>
{
    ColorByte IDeserialize<ColorByte>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var de = deserializer.ReadType(serdeInfo);
        var (index, errorName) = de.TryReadIndexWithName(serdeInfo);
        if (index == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        ColorByte _l_result;
        if (index == ITypeDeserializer.EndOfType)
        {
            // Assume we want to read the underlying value
            _l_result = (ColorByte)de.ReadU8(serdeInfo, index);
        }
        else
        {
            _l_result = index switch {
                0 => ColorByte.Red,
                1 => ColorByte.Green,
                2 => ColorByte.Blue,
                _ => throw new InvalidOperationException($"Unexpected index: {index}")
            };
        }
        de.End(serdeInfo);
        return _l_result;
    }
}
