//HintName: Test.ChannelList.IDeserializeProvider.g.cs

namespace Test;

partial record struct ChannelList : Serde.IDeserializeProvider<Test.ChannelList>
{
    static global::Serde.IDeserialize<Test.ChannelList> global::Serde.IDeserializeProvider<Test.ChannelList>.Instance { get; }
        = new Test.ChannelList._DeObj();
}
