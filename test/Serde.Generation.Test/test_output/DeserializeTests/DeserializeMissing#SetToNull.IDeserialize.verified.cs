//HintName: SetToNull.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record struct SetToNull : Serde.IDeserialize<SetToNull>
{
    static SetToNull Serde.IDeserialize<SetToNull>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "Present",
            "Missing",
            "ThrowMissing"
        };
        return deserializer.DeserializeType("SetToNull", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<SetToNull>
    {
        public string ExpectedTypeName => "SetToNull";

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
                    case (byte)'p'when s.SequenceEqual("present"u8):
                        return 1;
                    case (byte)'m'when s.SequenceEqual("missing"u8):
                        return 2;
                    case (byte)'t'when s.SequenceEqual("throwMissing"u8):
                        return 3;
                    default:
                        return 0;
                }
            }
        }

        SetToNull Serde.IDeserializeVisitor<SetToNull>.VisitDictionary<D>(ref D d)
        {
            string _l_present = default !;
            string? _l_missing = default !;
            string? _l_throwmissing = default !;
            byte _r_assignedValid = 0b10;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_present = d.GetNextValue<string, StringWrap>();
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 2:
                        _l_missing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 3:
                        _l_throwmissing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                }
            }

            if (_r_assignedValid != 0b111)
            {
                throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
            }

            var newType = new SetToNull()
            {
                Present = _l_present,
                Missing = _l_missing,
                ThrowMissing = _l_throwmissing,
            };
            return newType;
        }
    }
}