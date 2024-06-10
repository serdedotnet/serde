namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class DenyUnknownSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("present", typeof(Serde.Test.JsonDeserializeTests.DenyUnknown).GetProperty("Present")!),
("missing", typeof(Serde.Test.JsonDeserializeTests.DenyUnknown).GetProperty("Missing")!)
    });
}
}