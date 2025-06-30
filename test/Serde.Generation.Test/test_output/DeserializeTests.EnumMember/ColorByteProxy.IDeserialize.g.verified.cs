//HintName: ColorByteProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class ColorByteProxy : Serde.IDeserialize<ColorByte>
{
    async global::System.Threading.Tasks.Task<ColorByte> IDeserialize<ColorByte>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var de = deserializer.ReadType(serdeInfo);
        var (index, errorName) = await de.TryReadIndexWithName(serdeInfo);
        if (index == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        if (index == ITypeDeserializer.EndOfType)
        {
            // Assume we want to read the underlying value
            return (ColorByte)(await de.ReadU8(serdeInfo, index));
        }
        return index switch {
            0 => ColorByte.Red,
            1 => ColorByte.Green,
            2 => ColorByte.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
}
