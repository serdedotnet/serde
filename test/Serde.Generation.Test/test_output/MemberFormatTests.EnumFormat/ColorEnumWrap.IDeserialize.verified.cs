//HintName: ColorEnumWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record struct ColorEnumWrap : Serde.IDeserialize<ColorEnum>
{
    static ColorEnum Serde.IDeserialize<ColorEnum>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        return deserializer.DeserializeString(visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ColorEnum>
    {
        public string ExpectedTypeName => "ColorEnum";

        ColorEnum Serde.IDeserializeVisitor<ColorEnum>.VisitString(string s) => s switch
        {
            "Red" => ColorEnum.Red,
            "Green" => ColorEnum.Green,
            "Blue" => ColorEnum.Blue,
            _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + s)};
        ColorEnum Serde.IDeserializeVisitor<ColorEnum>.VisitUtf8Span(System.ReadOnlySpan<byte> s) => s switch
        {
            _ when System.MemoryExtensions.SequenceEqual(s, "Red"u8) => ColorEnum.Red,
            _ when System.MemoryExtensions.SequenceEqual(s, "Green"u8) => ColorEnum.Green,
            _ when System.MemoryExtensions.SequenceEqual(s, "Blue"u8) => ColorEnum.Blue,
            _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + System.Text.Encoding.UTF8.GetString(s))};
    }
}