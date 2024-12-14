//HintName: ColorLongProxy.IDeserialize.cs

#nullable enable
using System;
using Serde;

sealed partial class ColorLongProxy : Serde.IDeserialize<ColorLong>, Serde.IDeserializeProvider<ColorLong>
{
    ColorLong IDeserialize<ColorLong>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorLongProxy>();
        var de = deserializer.ReadType(serdeInfo);
        int index;
        if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }

        return index switch
        {
            0 => ColorLong.Red,
            1 => ColorLong.Green,
            2 => ColorLong.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")};
    }

    static IDeserialize<ColorLong> IDeserializeProvider<ColorLong>.DeserializeInstance => ColorLongProxy.Instance;
}