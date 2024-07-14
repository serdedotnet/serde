namespace Serde.Test;
partial class GenericWrapperTests
{
    internal static class CustomImArrayExplicitWrapOnMemberSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "CustomImArrayExplicitWrapOnMember",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("a", typeof(Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember).GetField("A")!)
    });
}
}