//HintName: Test.ChannelSerdeTypeInfo.cs
namespace Test;
internal static class ChannelSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("a", typeof(Test.Channel).GetField("A")!),
("b", typeof(Test.Channel).GetField("B")!),
("c", typeof(Test.Channel).GetField("C")!)
    });
}