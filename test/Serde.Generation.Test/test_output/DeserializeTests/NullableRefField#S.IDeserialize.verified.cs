﻿//HintName: S.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.IDeserialize<S>
{
    static S Serde.IDeserialize<S>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "F"
        };
        return deserializer.DeserializeType<S, SerdeVisitor>("S", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<S>
    {
        public string ExpectedTypeName => "S";

        private sealed class FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
        {
            public static byte Deserialize<D>(ref D deserializer)
                where D : IDeserializer => deserializer.DeserializeString<byte, FieldNameVisitor>(new FieldNameVisitor());
            public string ExpectedTypeName => "string";

            byte Serde.IDeserializeVisitor<byte>.VisitString(string s) => VisitUtf8Span(System.Text.Encoding.UTF8.GetBytes(s));
            public byte VisitUtf8Span(System.ReadOnlySpan<byte> s)
            {
                switch (s[0])
                {
                    case (byte)'f'when s.SequenceEqual("f"u8):
                        return 1;
                    default:
                        return 0;
                }
            }
        }

        S Serde.IDeserializeVisitor<S>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<string?> f = default;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        f = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        break;
                }
            }

            var newType = new S()
            {
                F = f.GetValueOrDefault(null),
            };
            return newType;
        }
    }
}