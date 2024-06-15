namespace Serde.Test;
partial class GenericWrapperTests
{
    internal static class CustomImArrayExplicitWrapOnMemberSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("a", typeof(Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember).GetField("A")!)
    });
}
}