namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class ThrowMissingFalseSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "ThrowMissingFalse",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("present", typeof(Serde.Test.JsonDeserializeTests.ThrowMissingFalse).GetProperty("Present")!),
("missing", typeof(Serde.Test.JsonDeserializeTests.ThrowMissingFalse).GetProperty("Missing")!)
    });
}
}