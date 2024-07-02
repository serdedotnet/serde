//HintName: ColorIntWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct ColorIntWrap : Serde.IDeserialize<ColorInt>
{
    static ColorInt IDeserialize<ColorInt>.Deserialize(IDeserializer deserializer)
    {
        var typeInfo = ColorIntSerdeTypeInfo.TypeInfo;
        var de = deserializer.DeserializeType(typeInfo);
        int index;
        if ((index = de.TryReadIndex(typeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
        {
            throw new InvalidDeserializeValueException($"Unexpected value: {errorName}");
        }

        return index switch
        {
            0 => ColorInt.Red,
            1 => ColorInt.Green,
            2 => ColorInt.Blue,
            _ => throw new InvalidDeserializeValueException($"Unexpected index: {index}")};
    }
}