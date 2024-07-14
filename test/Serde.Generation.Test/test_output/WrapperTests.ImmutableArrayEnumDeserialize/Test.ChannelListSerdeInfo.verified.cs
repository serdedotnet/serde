//HintName: Test.ChannelListSerdeInfo.cs
namespace Test;
internal static class ChannelListSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ChannelList",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("channels", typeof(Test.ChannelList).GetProperty("Channels")!)
    });
}