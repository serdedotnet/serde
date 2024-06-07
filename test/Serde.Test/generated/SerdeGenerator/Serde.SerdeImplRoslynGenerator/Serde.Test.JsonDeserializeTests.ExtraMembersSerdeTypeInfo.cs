namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class ExtraMembersSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("b", typeof(ExtraMembers).GetField("b")!)
    });
}
}