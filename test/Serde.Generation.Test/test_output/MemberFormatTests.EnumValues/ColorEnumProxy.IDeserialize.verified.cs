//HintName: ColorEnumProxy.IDeserialize.cs

#nullable enable

using System;
using Serde;
sealed partial class ColorEnumProxy :Serde.IDeserialize<ColorEnum>,Serde.IDeserializeProvider<ColorEnum>
{
    ColorEnum IDeserialize<ColorEnum>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorEnumProxy>();
        var de = deserializer.ReadType(serdeInfo);
        int index;
        if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        return index switch {
            0 => ColorEnum.Red,
            1 => ColorEnum.Green,
            2 => ColorEnum.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
    static IDeserialize<ColorEnum> IDeserializeProvider<ColorEnum>.DeserializeInstance
        => ColorEnumProxy.Instance;

}