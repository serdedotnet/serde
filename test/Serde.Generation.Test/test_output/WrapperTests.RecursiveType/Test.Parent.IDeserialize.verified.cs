//HintName: Test.Parent.IDeserialize.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial record Parent : Serde.IDeserialize<Test.Parent>
    {
        static Test.Parent Serde.IDeserialize<Test.Parent>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]
            {
                "R"
            };
            return deserializer.DeserializeType("Parent", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Test.Parent>
        {
            public string ExpectedTypeName => "Test.Parent";

            private sealed class FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
            {
                public static readonly FieldNameVisitor Instance = new FieldNameVisitor();
                public static byte Deserialize<D>(ref D deserializer)
                    where D : IDeserializer => deserializer.DeserializeString(Instance);
                public string ExpectedTypeName => "string";

                byte Serde.IDeserializeVisitor<byte>.VisitString(string s) => VisitUtf8Span(System.Text.Encoding.UTF8.GetBytes(s));
                public byte VisitUtf8Span(System.ReadOnlySpan<byte> s)
                {
                    switch (s[0])
                    {
                        case (byte)'r'when s.SequenceEqual("r"u8):
                            return 1;
                        default:
                            return 0;
                    }
                }
            }

            Test.Parent Serde.IDeserializeVisitor<Test.Parent>.VisitDictionary<D>(ref D d)
            {
                Recursive _l_r = default !;
                byte _r_assignedValid = 0b0;
                while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                {
                    switch (key)
                    {
                        case 1:
                            _l_r = d.GetNextValue<Recursive, Test.RecursiveWrap>();
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                    }
                }

                if (_r_assignedValid != 0b1)
                {
                    throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                }

                var newType = new Test.Parent()
                {
                    R = _l_r,
                };
                return newType;
            }
        }
    }
}