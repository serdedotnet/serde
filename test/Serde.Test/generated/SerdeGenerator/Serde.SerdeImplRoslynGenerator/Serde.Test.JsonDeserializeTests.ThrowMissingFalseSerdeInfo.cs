namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class ThrowMissingFalseSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ThrowMissingFalse",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("present", typeof(Serde.Test.JsonDeserializeTests.ThrowMissingFalse).GetProperty("Present")!),
("missing", typeof(Serde.Test.JsonDeserializeTests.ThrowMissingFalse).GetProperty("Missing")!)
    });
}
}