namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class SetToNullSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "SetToNull",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("present", typeof(Serde.Test.JsonDeserializeTests.SetToNull).GetProperty("Present")!),
("missing", typeof(Serde.Test.JsonDeserializeTests.SetToNull).GetProperty("Missing")!)
    });
}
}