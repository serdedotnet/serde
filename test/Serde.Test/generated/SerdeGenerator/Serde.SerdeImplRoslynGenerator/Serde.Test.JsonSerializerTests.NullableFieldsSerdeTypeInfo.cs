namespace Serde.Test;
partial class JsonSerializerTests
{
    internal static class NullableFieldsSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("s", typeof(Serde.Test.JsonSerializerTests.NullableFields).GetField("S")!),
("d", typeof(Serde.Test.JsonSerializerTests.NullableFields).GetField("D")!)
    });
}
}