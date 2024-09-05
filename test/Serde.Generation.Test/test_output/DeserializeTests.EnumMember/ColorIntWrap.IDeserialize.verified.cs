//HintName: ColorIntWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct ColorIntWrap : Serde.IDeserialize<ColorInt>
{
    static ColorInt IDeserialize<ColorInt>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorIntWrap>();
        var de = deserializer.ReadType(serdeInfo);
        int index;
        if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }

        return index switch
        {
            0 => ColorInt.Red,
            1 => ColorInt.Green,
            2 => ColorInt.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")};
    }
}