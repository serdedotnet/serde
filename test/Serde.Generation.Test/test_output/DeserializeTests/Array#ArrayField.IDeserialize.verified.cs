//HintName: ArrayField.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial class ArrayField : Serde.IDeserialize<ArrayField>
{
    static ArrayField Serde.IDeserialize<ArrayField>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "IntArr"
        };
        return deserializer.DeserializeType<ArrayField, SerdeVisitor>("ArrayField", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ArrayField>
    {
        public string ExpectedTypeName => "ArrayField";

        private struct FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
        {
            public static byte Deserialize<D>(ref D deserializer)
                where D : IDeserializer => deserializer.DeserializeString<byte, FieldNameVisitor>(new FieldNameVisitor());
            public string ExpectedTypeName => "string";

            byte Serde.IDeserializeVisitor<byte>.VisitString(string s) => VisitUtf8Span(System.Text.Encoding.UTF8.GetBytes(s));
            public byte VisitUtf8Span(System.ReadOnlySpan<byte> s)
            {
                switch (s[0])
                {
                    case (byte)'i'when s.SequenceEqual("intArr"u8):
                        return 1;
                    default:
                        return 0;
                }
            }
        }

        ArrayField Serde.IDeserializeVisitor<ArrayField>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<int[]> _l_intarr = default;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_intarr = d.GetNextValue<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                        break;
                }
            }

            var newType = new ArrayField()
            {
                IntArr = _l_intarr.GetValueOrThrow("IntArr"),
            };
            return newType;
        }
    }
}