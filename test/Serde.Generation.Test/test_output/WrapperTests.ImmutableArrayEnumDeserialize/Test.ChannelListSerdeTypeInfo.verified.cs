//HintName: Test.ChannelListSerdeTypeInfo.cs
namespace Test;
internal static class ChannelListSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<ChannelList>(nameof(ChannelList), new (string, System.Reflection.MemberInfo)[] {
        ("channels", typeof(ChannelList).GetProperty("Channels")!)
    });
}