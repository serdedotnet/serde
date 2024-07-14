namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class NullableFieldsSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "NullableFields",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("s", typeof(Serde.Test.JsonDeserializeTests.NullableFields).GetField("S")!),
("dict", typeof(Serde.Test.JsonDeserializeTests.NullableFields).GetField("Dict")!)
    });
}
}