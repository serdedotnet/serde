﻿//HintName: S.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.IDeserialize<S>
{
    static S Serde.IDeserialize<S>.Deserialize(IDeserializer deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "E"
        };
        return deserializer.DeserializeType("S", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<S>
    {
        public string ExpectedTypeName => "S";

        private sealed class FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
        {
            public static readonly FieldNameVisitor Instance = new FieldNameVisitor();
            public static byte Deserialize(IDeserializer deserializer) => deserializer.DeserializeString(Instance);
            public string ExpectedTypeName => "string";

            byte Serde.IDeserializeVisitor<byte>.VisitString(string s) => VisitUtf8Span(System.Text.Encoding.UTF8.GetBytes(s));
            public byte VisitUtf8Span(System.ReadOnlySpan<byte> s)
            {
                switch (s[0])
                {
                    case (byte)'e'when s.SequenceEqual("e"u8):
                        return 1;
                    default:
                        return 0;
                }
            }
        }

        S Serde.IDeserializeVisitor<S>.VisitDictionary<D>(ref D d)
        {
            ColorEnum _l_e = default !;
            byte _r_assignedValid = 0b0;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_e = d.GetNextValue<ColorEnum, global::ColorEnumWrap>();
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                }
            }

            if (_r_assignedValid != 0b1)
            {
                throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
            }

            var newType = new S()
            {
                E = _l_e,
            };
            return newType;
        }
    }
}