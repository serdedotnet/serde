//HintName: Test.ChannelSerdeInfo.cs
namespace Test;
internal static class ChannelSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "Channel",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("a", typeof(Test.Channel).GetField("A")!),
("b", typeof(Test.Channel).GetField("B")!),
("c", typeof(Test.Channel).GetField("C")!)
    });
}