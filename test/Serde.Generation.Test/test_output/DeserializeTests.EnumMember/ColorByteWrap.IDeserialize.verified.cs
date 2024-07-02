//HintName: ColorByteWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct ColorByteWrap : Serde.IDeserialize<ColorByte>
{
    static ColorByte IDeserialize<ColorByte>.Deserialize(IDeserializer deserializer)
    {
        var typeInfo = ColorByteSerdeTypeInfo.TypeInfo;
        var de = deserializer.DeserializeType(typeInfo);
        int index;
        if ((index = de.TryReadIndex(typeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
        {
            throw new InvalidDeserializeValueException($"Unexpected value: {errorName}");
        }

        return index switch
        {
            0 => ColorByte.Red,
            1 => ColorByte.Green,
            2 => ColorByte.Blue,
            _ => throw new InvalidDeserializeValueException($"Unexpected index: {index}")};
    }
}