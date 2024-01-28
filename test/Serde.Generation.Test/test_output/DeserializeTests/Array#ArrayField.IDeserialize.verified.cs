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
        return deserializer.DeserializeType("ArrayField", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ArrayField>
    {
        public string ExpectedTypeName => "ArrayField";

        private struct FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
        {
            public static byte Deserialize<D>(ref D deserializer)
                where D : IDeserializer => deserializer.DeserializeString(new FieldNameVisitor());
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
            int[] _l_intarr = default !;
            byte _r_assignedValid = 0b0;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_intarr = d.GetNextValue<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                }
            }

            if (_r_assignedValid != 0b1)
            {
                throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
            }

            var newType = new ArrayField()
            {
                IntArr = _l_intarr,
            };
            return newType;
        }
    }
}