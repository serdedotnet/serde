namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class IdStructSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "IdStruct",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("id", typeof(Serde.Test.JsonDeserializeTests.IdStruct).GetField("Id")!)
    });
}
}