//HintName: Test.ChannelList.IDeserialize.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial record struct ChannelList : Serde.IDeserialize<Test.ChannelList>
    {
        static Test.ChannelList Serde.IDeserialize<Test.ChannelList>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]
            {
                "Channels"
            };
            return deserializer.DeserializeType("ChannelList", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Test.ChannelList>
        {
            public string ExpectedTypeName => "Test.ChannelList";

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
                        case (byte)'c'when s.SequenceEqual("channels"u8):
                            return 1;
                        default:
                            return 0;
                    }
                }
            }

            Test.ChannelList Serde.IDeserializeVisitor<Test.ChannelList>.VisitDictionary<D>(ref D d)
            {
                System.Collections.Immutable.ImmutableArray<Test.Channel> _l_channels = default !;
                byte _r_assignedValid = 0b0;
                while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                {
                    switch (key)
                    {
                        case 1:
                            _l_channels = d.GetNextValue<System.Collections.Immutable.ImmutableArray<Test.Channel>, ImmutableArrayWrap.DeserializeImpl<Test.Channel, Test.ChannelWrap>>();
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                    }
                }

                if (_r_assignedValid != 0b1)
                {
                    throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                }

                var newType = new Test.ChannelList()
                {
                    Channels = _l_channels,
                };
                return newType;
            }
        }
    }
}