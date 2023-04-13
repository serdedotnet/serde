//HintName: Test.ChannelList.IDeserialize.cs

#nullable enable
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

            Test.ChannelList Serde.IDeserializeVisitor<Test.ChannelList>.VisitDictionary<D>(ref D d)
            {
                Serde.Option<System.Collections.Immutable.ImmutableArray<Test.Channel>> channels = default;
                while (d.TryGetNextKey<string, StringWrap>(out string? key))
                {
                    switch (key)
                    {
                        case "channels":
                            channels = d.GetNextValue<System.Collections.Immutable.ImmutableArray<Test.Channel>, ImmutableArrayWrap.DeserializeImpl<Test.Channel, ChannelWrap>>();
                            break;
                        default:
                            break;
                    }
                }

                var newType = new Test.ChannelList()
                {
                    Channels = channels.GetValueOrThrow("Channels"),
                };
                return newType;
            }
        }
    }
}