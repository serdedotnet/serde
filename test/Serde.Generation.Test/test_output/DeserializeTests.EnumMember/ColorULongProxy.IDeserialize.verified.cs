//HintName: ColorULongProxy.IDeserialize.cs

#nullable enable

using System;
using Serde;
sealed partial class ColorULongProxy :Serde.IDeserialize<ColorULong>,Serde.IDeserializeProvider<ColorULong>
{
    ColorULong IDeserialize<ColorULong>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var de = deserializer.ReadType(serdeInfo);
        int index;
        if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        return index switch {
            0 => ColorULong.Red,
            1 => ColorULong.Green,
            2 => ColorULong.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
    static IDeserialize<ColorULong> IDeserializeProvider<ColorULong>.Instance
        => ColorULongProxy.Instance;

}