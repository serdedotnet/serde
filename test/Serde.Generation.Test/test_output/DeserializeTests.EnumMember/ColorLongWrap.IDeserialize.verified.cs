//HintName: ColorLongWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record struct ColorLongWrap : Serde.IDeserialize<ColorLong>
{
    static ColorLong Serde.IDeserialize<ColorLong>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        return deserializer.DeserializeString<ColorLong, SerdeVisitor>(visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ColorLong>
    {
        public string ExpectedTypeName => "ColorLong";

        ColorLong Serde.IDeserializeVisitor<ColorLong>.VisitString(string s) => s switch
        {
            "red" => ColorLong.Red,
            "green" => ColorLong.Green,
            "blue" => ColorLong.Blue,
            _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + s)};
        ColorLong Serde.IDeserializeVisitor<ColorLong>.VisitUtf8Span(System.ReadOnlySpan<byte> s) => s switch
        {
            _ when System.MemoryExtensions.SequenceEqual(s, "red"u8) => ColorLong.Red,
            _ when System.MemoryExtensions.SequenceEqual(s, "green"u8) => ColorLong.Green,
            _ when System.MemoryExtensions.SequenceEqual(s, "blue"u8) => ColorLong.Blue,
            _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + System.Text.Encoding.UTF8.GetString(s))};
    }
}