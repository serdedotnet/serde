//HintName: ColorByteProxy.IDeserialize.cs

#nullable enable
using System;
using Serde;

sealed partial class ColorByteProxy : Serde.IDeserialize<ColorByte>, Serde.IDeserializeProvider<ColorByte>
{
    ColorByte IDeserialize<ColorByte>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorByteProxy>();
        var de = deserializer.ReadType(serdeInfo);
        int index;
        if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }

        return index switch
        {
            0 => ColorByte.Red,
            1 => ColorByte.Green,
            2 => ColorByte.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")};
    }

    static IDeserialize<ColorByte> IDeserializeProvider<ColorByte>.DeserializeInstance => ColorByteProxy.Instance;
}