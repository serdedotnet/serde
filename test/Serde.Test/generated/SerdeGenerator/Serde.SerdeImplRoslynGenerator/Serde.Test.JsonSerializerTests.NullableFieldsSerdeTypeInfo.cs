namespace Serde.Test;
partial class JsonSerializerTests
{
    internal static class NullableFieldsSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("s", typeof(NullableFields).GetField("S")!),
("d", typeof(NullableFields).GetField("D")!)
    });
}
}