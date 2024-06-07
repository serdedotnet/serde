namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class DenyUnknownSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("present", typeof(DenyUnknown).GetProperty("Present")!),
("missing", typeof(DenyUnknown).GetProperty("Missing")!)
    });
}
}