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
        var de = deserializer.ReadType(serdeInfo);
        var (index, errorName) = de.TryReadIndexWithName(serdeInfo);
        if (index == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        Test.Channel _l_result;
        if (index == ITypeDeserializer.EndOfType)
        {
            // Assume we want to read the underlying value
            _l_result = (Test.Channel)de.ReadI32(serdeInfo, index);
        }
        else
        {
            _l_result = index switch {
                0 => Test.Channel.A,
                1 => Test.Channel.B,
                2 => Test.Channel.C,
                _ => throw new InvalidOperationException($"Unexpected index: {index}")
            };
        }
        de.End(serdeInfo);
        return _l_result;
    }
}
