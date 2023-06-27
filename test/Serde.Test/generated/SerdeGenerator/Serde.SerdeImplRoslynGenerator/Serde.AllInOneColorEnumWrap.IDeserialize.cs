
#nullable enable
using System;
using Serde;

namespace Serde
{
    partial record struct AllInOneColorEnumWrap : Serde.IDeserialize<Serde.Test.AllInOne.ColorEnum>
    {
        static Serde.Test.AllInOne.ColorEnum Serde.IDeserialize<Serde.Test.AllInOne.ColorEnum>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            return deserializer.DeserializeString<Serde.Test.AllInOne.ColorEnum, SerdeVisitor>(visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.AllInOne.ColorEnum>
        {
            public string ExpectedTypeName => "Serde.Test.AllInOne.ColorEnum";

            Serde.Test.AllInOne.ColorEnum Serde.IDeserializeVisitor<Serde.Test.AllInOne.ColorEnum>.VisitString(string s) => s switch
            {
                "red" => Serde.Test.AllInOne.ColorEnum.Red,
                "blue" => Serde.Test.AllInOne.ColorEnum.Blue,
                "green" => Serde.Test.AllInOne.ColorEnum.Green,
                _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + s)};
            Serde.Test.AllInOne.ColorEnum Serde.IDeserializeVisitor<Serde.Test.AllInOne.ColorEnum>.VisitUtf8Span(System.ReadOnlySpan<byte> s) => s switch
            {
                _ when System.MemoryExtensions.SequenceEqual(s, "red"u8) => Serde.Test.AllInOne.ColorEnum.Red,
                _ when System.MemoryExtensions.SequenceEqual(s, "blue"u8) => Serde.Test.AllInOne.ColorEnum.Blue,
                _ when System.MemoryExtensions.SequenceEqual(s, "green"u8) => Serde.Test.AllInOne.ColorEnum.Green,
                _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + System.Text.Encoding.UTF8.GetString(s))};
        }
    }
}