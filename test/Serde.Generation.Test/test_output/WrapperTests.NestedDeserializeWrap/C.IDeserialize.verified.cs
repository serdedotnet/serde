//HintName: C.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.IDeserialize<C>
{
    static C Serde.IDeserialize<C>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "S"
        };
        return deserializer.DeserializeType("C", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<C>
    {
        public string ExpectedTypeName => "C";

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
                    case (byte)'s'when s.SequenceEqual("s"u8):
                        return 1;
                    default:
                        return 0;
                }
            }
        }

        C Serde.IDeserializeVisitor<C>.VisitDictionary<D>(ref D d)
        {
            System.Runtime.InteropServices.ComTypes.BIND_OPTS _l_s = default !;
            byte _r_assignedValid = 0b0;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_s = d.GetNextValue<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OPTSWrap>();
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                }
            }

            if (_r_assignedValid != 0b1)
            {
                throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
            }

            var newType = new C()
            {
                S = _l_s,
            };
            return newType;
        }
    }
}