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
            var typeInfo = Test.ChannelSerdeTypeInfo.TypeInfo;
            var de = deserializer.DeserializeType(typeInfo);
            int index;
            if ((index = de.TryReadIndex(typeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
            {
                throw new InvalidDeserializeValueException($"Unexpected value: {errorName}");
            }

            return index switch
            {
                0 => Test.Channel.A,
                1 => Test.Channel.B,
                2 => Test.Channel.C,
                _ => throw new InvalidDeserializeValueException($"Unexpected index: {index}")};
        }
    }
}