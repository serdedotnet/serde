namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class IdStructListSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "IdStructList",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("count", typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetProperty("Count")!),
("list", typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetProperty("List")!)
    });
}
}