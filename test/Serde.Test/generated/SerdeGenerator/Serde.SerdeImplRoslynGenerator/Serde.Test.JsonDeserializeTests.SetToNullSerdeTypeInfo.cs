namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class SetToNullSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("present", typeof(SetToNull).GetProperty("Present")!),
("missing", typeof(SetToNull).GetProperty("Missing")!)
    });
}
}