namespace Serde.Test;
partial class JsonSerializerTests
{
    internal static class NullableFieldsSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "NullableFields",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("s", typeof(Serde.Test.JsonSerializerTests.NullableFields).GetField("S")!),
("d", typeof(Serde.Test.JsonSerializerTests.NullableFields).GetField("D")!)
    });
}
}