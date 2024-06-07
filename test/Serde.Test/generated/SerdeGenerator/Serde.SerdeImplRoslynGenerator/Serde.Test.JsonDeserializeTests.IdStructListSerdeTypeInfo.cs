namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class IdStructListSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("count", typeof(IdStructList).GetProperty("Count")!),
("list", typeof(IdStructList).GetProperty("List")!)
    });
}
}