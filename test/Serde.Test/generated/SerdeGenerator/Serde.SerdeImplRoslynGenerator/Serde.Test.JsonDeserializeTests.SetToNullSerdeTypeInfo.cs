namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class SetToNullSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "SetToNull",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("present", typeof(Serde.Test.JsonDeserializeTests.SetToNull).GetProperty("Present")!),
("missing", typeof(Serde.Test.JsonDeserializeTests.SetToNull).GetProperty("Missing")!)
    });
}
}