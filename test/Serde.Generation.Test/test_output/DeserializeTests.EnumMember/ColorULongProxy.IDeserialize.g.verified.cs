//HintName: ColorULongProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class ColorULongProxy : Serde.IDeserialize<ColorULong>
{
    async global::System.Threading.Tasks.ValueTask<ColorULong> IDeserialize<ColorULong>.Deserialize(IDeserializer deserializer)
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
            return (ColorULong)(await de.ReadU64(serdeInfo, index));
        }
        return index switch {
            0 => ColorULong.Red,
            1 => ColorULong.Green,
            2 => ColorULong.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
}
