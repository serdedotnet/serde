//HintName: ColorEnumProxy.IDeserialize.cs

#nullable enable

using System;
using Serde;
sealed partial class ColorEnumProxy :Serde.IDeserialize<ColorEnum>,Serde.IDeserializeProvider<ColorEnum>
{
    ColorEnum IDeserialize<ColorEnum>.Deserialize(IDeserializer deserializer)
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
            return (ColorEnum)de.ReadI32(serdeInfo, index);
        }
        return index switch {
            0 => ColorEnum.Red,
            1 => ColorEnum.Green,
            2 => ColorEnum.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
    static IDeserialize<ColorEnum> IDeserializeProvider<ColorEnum>.Instance
        => ColorEnumProxy.Instance;

}