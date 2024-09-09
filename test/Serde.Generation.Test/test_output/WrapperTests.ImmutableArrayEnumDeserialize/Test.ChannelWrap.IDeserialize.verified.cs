//HintName: Test.ChannelWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial struct ChannelWrap : Serde.IDeserialize<Test.Channel>
    {
        static Test.Channel IDeserialize<Test.Channel>.Deserialize(IDeserializer deserializer)
        {
            var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Test.ChannelWrap>();
            var de = deserializer.ReadType(serdeInfo);
            int index;
            if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
            {
                throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
            }

            return index switch
            {
                0 => Test.Channel.A,
                1 => Test.Channel.B,
                2 => Test.Channel.C,
                _ => throw new InvalidOperationException($"Unexpected index: {index}")};
        }
    }
}