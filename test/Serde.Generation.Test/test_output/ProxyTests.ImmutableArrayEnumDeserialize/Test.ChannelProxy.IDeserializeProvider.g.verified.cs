//HintName: Test.ChannelProxy.IDeserializeProvider.g.cs

namespace Test;

partial class ChannelProxy : Serde.IDeserializeProvider<Test.Channel>
{
    static global::Serde.IDeserialize<Test.Channel> global::Serde.IDeserializeProvider<Test.Channel>.Instance { get; }
        = new Test.ChannelProxy();
}
