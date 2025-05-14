//HintName: ColorIntProxy.IDeserialize.cs

#nullable enable

using System;
using Serde;
sealed partial class ColorIntProxy :Serde.IDeserialize<ColorInt>,Serde.IDeserializeProvider<ColorInt>
{
    ColorInt IDeserialize<ColorInt>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var de = deserializer.ReadType(serdeInfo);
        int index = de.TryReadIndex(serdeInfo, out var errorName);
        if (index == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        if (index == ITypeDeserializer.EndOfType)
        {
            // Assume we want to read the underlying value
            return (ColorInt)de.ReadI32(serdeInfo, index);
        }
        return index switch {
            0 => ColorInt.Red,
            1 => ColorInt.Green,
            2 => ColorInt.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
    static IDeserialize<ColorInt> IDeserializeProvider<ColorInt>.Instance
        => ColorIntProxy.Instance;

}