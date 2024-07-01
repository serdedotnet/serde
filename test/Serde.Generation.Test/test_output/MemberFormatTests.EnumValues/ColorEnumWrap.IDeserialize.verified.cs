//HintName: ColorEnumWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct ColorEnumWrap : Serde.IDeserialize<ColorEnum>
{
    static ColorEnum Serde.IDeserialize<ColorEnum>.Deserialize(IDeserializer deserializer)
    {
        var visitor = new SerdeVisitor();
        return deserializer.DeserializeString(visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ColorEnum>
    {
        public string ExpectedTypeName => "ColorEnum";

        ColorEnum Serde.IDeserializeVisitor<ColorEnum>.VisitString(string s) => s switch
        {
            "red" => ColorEnum.Red,
            "green" => ColorEnum.Green,
            "blue" => ColorEnum.Blue,
            _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + s)};
        ColorEnum Serde.IDeserializeVisitor<ColorEnum>.VisitUtf8Span(System.ReadOnlySpan<byte> s) => s switch
        {
            _ when System.MemoryExtensions.SequenceEqual(s, "red"u8) => ColorEnum.Red,
            _ when System.MemoryExtensions.SequenceEqual(s, "green"u8) => ColorEnum.Green,
            _ when System.MemoryExtensions.SequenceEqual(s, "blue"u8) => ColorEnum.Blue,
            _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + System.Text.Encoding.UTF8.GetString(s))};
    }
}