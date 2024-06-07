//HintName: Test.ChannelSerdeTypeInfo.cs
namespace Test;
internal static class ChannelSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("a", typeof(Channel).GetField("A")!),
("b", typeof(Channel).GetField("B")!),
("c", typeof(Channel).GetField("C")!)
    });
}