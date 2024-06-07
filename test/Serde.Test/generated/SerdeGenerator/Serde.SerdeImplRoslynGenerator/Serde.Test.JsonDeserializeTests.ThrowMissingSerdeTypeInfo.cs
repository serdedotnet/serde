namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class ThrowMissingSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("present", typeof(ThrowMissing).GetProperty("Present")!),
("missing", typeof(ThrowMissing).GetProperty("Missing")!)
    });
}
}