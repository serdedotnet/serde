namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class SkipDeserializeSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("required", typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetProperty("Required")!),
("skip", typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetProperty("Skip")!)
    });
}
}