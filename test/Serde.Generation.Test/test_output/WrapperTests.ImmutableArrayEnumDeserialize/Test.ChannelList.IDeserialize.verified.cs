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
            return deserializer.DeserializeType<Test.ChannelList, SerdeVisitor>("ChannelList", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Test.ChannelList>
        {
            public string ExpectedTypeName => "Test.ChannelList";

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
                        case (byte)'c'when s.SequenceEqual("channels"u8):
                            return 1;
                        default:
                            return 0;
                    }
                }
            }

            Test.ChannelList Serde.IDeserializeVisitor<Test.ChannelList>.VisitDictionary<D>(ref D d)
            {
                Serde.Option<System.Collections.Immutable.ImmutableArray<Test.Channel>> _l_channels = default;
                while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                {
                    switch (key)
                    {
                        case 1:
                            _l_channels = d.GetNextValue<System.Collections.Immutable.ImmutableArray<Test.Channel>, ImmutableArrayWrap.DeserializeImpl<Test.Channel, ChannelWrap>>();
                            break;
                    }
                }

                var newType = new Test.ChannelList()
                {
                    Channels = _l_channels.GetValueOrThrow("Channels"),
                };
                return newType;
            }
        }
    }
}