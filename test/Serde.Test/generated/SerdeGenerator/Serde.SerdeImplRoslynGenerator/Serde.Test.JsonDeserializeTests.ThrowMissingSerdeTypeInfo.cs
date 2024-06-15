namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class ThrowMissingSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "ThrowMissing",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("present", typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetProperty("Present")!),
("missing", typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetProperty("Missing")!)
    });
}
}