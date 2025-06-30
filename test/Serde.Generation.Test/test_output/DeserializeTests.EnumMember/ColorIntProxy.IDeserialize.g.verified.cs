//HintName: ColorIntProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class ColorIntProxy : Serde.IDeserialize<ColorInt>
{
    async global::System.Threading.Tasks.ValueTask<ColorInt> IDeserialize<ColorInt>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var de = deserializer.ReadType(serdeInfo);
        int index = await de.TryReadIndex(serdeInfo, out var errorName);
        if (index == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        if (index == ITypeDeserializer.EndOfType)
        {
            // Assume we want to read the underlying value
            return (ColorInt)(await de.ReadI32(serdeInfo, index));
        }
        return index switch {
            0 => ColorInt.Red,
            1 => ColorInt.Green,
            2 => ColorInt.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
}
