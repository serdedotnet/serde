namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class IdStructSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("id", typeof(IdStruct).GetField("Id")!)
    });
}
}