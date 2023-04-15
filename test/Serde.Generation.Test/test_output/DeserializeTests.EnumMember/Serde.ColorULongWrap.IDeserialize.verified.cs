//HintName: Serde.ColorULongWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

namespace Serde
{
    partial record struct ColorULongWrap : Serde.IDeserialize<ColorULong>
    {
        static ColorULong Serde.IDeserialize<ColorULong>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            return deserializer.DeserializeString<ColorULong, SerdeVisitor>(visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ColorULong>
        {
            public string ExpectedTypeName => "ColorULong";

            ColorULong Serde.IDeserializeVisitor<ColorULong>.VisitString(string s) => s switch
            {
                "red" => ColorULong.Red,
                "green" => ColorULong.Green,
                "blue" => ColorULong.Blue,
                _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + s)};
            ColorULong Serde.IDeserializeVisitor<ColorULong>.VisitUtf8Span(System.ReadOnlySpan<byte> s) => s switch
            {
                _ when System.MemoryExtensions.SequenceEqual(s, "red"u8) => ColorULong.Red,
                _ when System.MemoryExtensions.SequenceEqual(s, "green"u8) => ColorULong.Green,
                _ when System.MemoryExtensions.SequenceEqual(s, "blue"u8) => ColorULong.Blue,
                _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + System.Text.Encoding.UTF8.GetString(s))};
        }
    }
}