//HintName: Test.ChannelProxy.IDeserialize.cs

#nullable enable

using System;
using Serde;

namespace Test;

sealed partial class ChannelProxy :Serde.IDeserialize<Test.Channel>,Serde.IDeserializeProvider<Test.Channel>
{
    Test.Channel IDeserialize<Test.Channel>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var de = deserializer.ReadType(serdeInfo);
        int index;
        if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        return index switch {
            0 => Test.Channel.A,
            1 => Test.Channel.B,
            2 => Test.Channel.C,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
    static IDeserialize<Test.Channel> IDeserializeProvider<Test.Channel>.Instance
        => Test.ChannelProxy.Instance;

}