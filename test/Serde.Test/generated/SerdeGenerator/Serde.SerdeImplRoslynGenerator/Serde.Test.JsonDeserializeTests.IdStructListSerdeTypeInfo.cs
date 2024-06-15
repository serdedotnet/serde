namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class IdStructListSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "IdStructList",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("count", typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetProperty("Count")!),
("list", typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetProperty("List")!)
    });
}
}