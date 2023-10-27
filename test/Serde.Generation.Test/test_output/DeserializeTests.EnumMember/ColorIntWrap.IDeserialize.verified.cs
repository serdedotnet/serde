//HintName: ColorIntWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record struct ColorIntWrap : Serde.IDeserialize<ColorInt>
{
    static ColorInt Serde.IDeserialize<ColorInt>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        return deserializer.DeserializeString<ColorInt, SerdeVisitor>(visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ColorInt>
    {
        public string ExpectedTypeName => "ColorInt";

        ColorInt Serde.IDeserializeVisitor<ColorInt>.VisitString(string s) => s switch
        {
            "red" => ColorInt.Red,
            "green" => ColorInt.Green,
            "blue" => ColorInt.Blue,
            _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + s)};
        ColorInt Serde.IDeserializeVisitor<ColorInt>.VisitUtf8Span(System.ReadOnlySpan<byte> s) => s switch
        {
            _ when System.MemoryExtensions.SequenceEqual(s, "red"u8) => ColorInt.Red,
            _ when System.MemoryExtensions.SequenceEqual(s, "green"u8) => ColorInt.Green,
            _ when System.MemoryExtensions.SequenceEqual(s, "blue"u8) => ColorInt.Blue,
            _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + System.Text.Encoding.UTF8.GetString(s))};
    }
}