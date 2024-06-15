namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class NullableFieldsSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "NullableFields",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("s", typeof(Serde.Test.JsonDeserializeTests.NullableFields).GetField("S")!),
("dict", typeof(Serde.Test.JsonDeserializeTests.NullableFields).GetField("Dict")!)
    });
}
}