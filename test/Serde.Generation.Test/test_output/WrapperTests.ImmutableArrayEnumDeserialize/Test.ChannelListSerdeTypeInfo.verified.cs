//HintName: Test.ChannelListSerdeTypeInfo.cs
namespace Test;
internal static class ChannelListSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "ChannelList",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("channels", typeof(Test.ChannelList).GetProperty("Channels")!)
    });
}