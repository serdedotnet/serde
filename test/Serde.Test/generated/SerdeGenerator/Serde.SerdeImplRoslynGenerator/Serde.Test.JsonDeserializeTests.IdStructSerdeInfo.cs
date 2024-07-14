namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class IdStructSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "IdStruct",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("id", typeof(Serde.Test.JsonDeserializeTests.IdStruct).GetField("Id")!)
    });
}
}