namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class SkipDeserializeSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "SkipDeserialize",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("required", typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetProperty("Required")!),
("skip", typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetProperty("Skip")!)
    });
}
}