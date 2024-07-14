namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class ExtraMembersSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ExtraMembers",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("b", typeof(Serde.Test.JsonDeserializeTests.ExtraMembers).GetField("b")!)
    });
}
}