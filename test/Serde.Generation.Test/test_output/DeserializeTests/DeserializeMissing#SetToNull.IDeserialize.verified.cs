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
            Serde.Option<string> present = default;
            Serde.Option<string?> missing = default;
            Serde.Option<string?> throwmissing = default;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        present = d.GetNextValue<string, StringWrap>();
                        break;
                    case 2:
                        missing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        break;
                    case 3:
                        throwmissing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        break;
                }
            }

            var newType = new SetToNull()
            {
                Present = present.GetValueOrThrow("Present"),
                Missing = missing.GetValueOrDefault(null),
                ThrowMissing = throwmissing.GetValueOrThrow("ThrowMissing"),
            };
            return newType;
        }
    }
}