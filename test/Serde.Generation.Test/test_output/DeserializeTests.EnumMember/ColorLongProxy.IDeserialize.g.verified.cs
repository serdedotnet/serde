//HintName: ColorLongProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class ColorLongProxy : Serde.IDeserialize<ColorLong>
{
    async global::System.Threading.Tasks.ValueTask<ColorLong> IDeserialize<ColorLong>.Deserialize(IDeserializer deserializer)
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
            return (ColorLong)(await de.ReadI64(serdeInfo, index));
        }
        return index switch {
            0 => ColorLong.Red,
            1 => ColorLong.Green,
            2 => ColorLong.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
}
