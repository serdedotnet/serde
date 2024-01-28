//HintName: ColorByteWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record struct ColorByteWrap : Serde.IDeserialize<ColorByte>
{
    static ColorByte Serde.IDeserialize<ColorByte>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        return deserializer.DeserializeString(visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ColorByte>
    {
        public string ExpectedTypeName => "ColorByte";

        ColorByte Serde.IDeserializeVisitor<ColorByte>.VisitString(string s) => s switch
        {
            "red" => ColorByte.Red,
            "green" => ColorByte.Green,
            "blue" => ColorByte.Blue,
            _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + s)};
        ColorByte Serde.IDeserializeVisitor<ColorByte>.VisitUtf8Span(System.ReadOnlySpan<byte> s) => s switch
        {
            _ when System.MemoryExtensions.SequenceEqual(s, "red"u8) => ColorByte.Red,
            _ when System.MemoryExtensions.SequenceEqual(s, "green"u8) => ColorByte.Green,
            _ when System.MemoryExtensions.SequenceEqual(s, "blue"u8) => ColorByte.Blue,
            _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + System.Text.Encoding.UTF8.GetString(s))};
    }
}