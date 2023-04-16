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
        return deserializer.DeserializeType<SetToNull, SerdeVisitor>("SetToNull", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<SetToNull>
    {
        public string ExpectedTypeName => "SetToNull";

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
            Serde.Option<string> _l_present = default;
            Serde.Option<string?> _l_missing = default;
            Serde.Option<string?> _l_throwmissing = default;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_present = d.GetNextValue<string, StringWrap>();
                        break;
                    case 2:
                        _l_missing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        break;
                    case 3:
                        _l_throwmissing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        break;
                }
            }

            var newType = new SetToNull()
            {
                Present = _l_present.GetValueOrThrow("Present"),
                Missing = _l_missing.GetValueOrDefault(null),
                ThrowMissing = _l_throwmissing.GetValueOrThrow("ThrowMissing"),
            };
            return newType;
        }
    }
}