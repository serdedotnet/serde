//HintName: Test.ChannelProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;

namespace Test;

partial class ChannelProxy : Serde.IDeserialize<Test.Channel>
{
    Test.Channel IDeserialize<Test.Channel>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var index = deserializer.ReadEnum(serdeInfo);
        Test.Channel _l_result = index switch {
            0 => Test.Channel.A,
            1 => Test.Channel.B,
            2 => Test.Channel.C,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
        return _l_result;
    }
}
