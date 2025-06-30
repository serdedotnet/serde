//HintName: Test.ChannelProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;

namespace Test;

partial class ChannelProxy : Serde.IDeserialize<Test.Channel>
{
    async global::System.Threading.Tasks.ValueTask<Test.Channel> IDeserialize<Test.Channel>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var de = deserializer.ReadType(serdeInfo);
        int index = await de.TryReadIndex(serdeInfo, out var errorName);
        if (index == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        if (index == ITypeDeserializer.EndOfType)
        {
            // Assume we want to read the underlying value
            return (Test.Channel)(await de.ReadI32(serdeInfo, index));
        }
        return index switch {
            0 => Test.Channel.A,
            1 => Test.Channel.B,
            2 => Test.Channel.C,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
}
