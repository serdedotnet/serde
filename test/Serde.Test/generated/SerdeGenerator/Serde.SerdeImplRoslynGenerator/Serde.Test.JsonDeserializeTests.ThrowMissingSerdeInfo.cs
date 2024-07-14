namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class ThrowMissingSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ThrowMissing",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("present", typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetProperty("Present")!),
("missing", typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetProperty("Missing")!)
    });
}
}