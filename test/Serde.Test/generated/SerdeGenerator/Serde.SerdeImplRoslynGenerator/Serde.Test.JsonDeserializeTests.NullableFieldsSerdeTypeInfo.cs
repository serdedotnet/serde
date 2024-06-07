namespace Serde.Test;
partial class JsonDeserializeTests
{
    internal static class NullableFieldsSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("s", typeof(NullableFields).GetField("S")!),
("dict", typeof(NullableFields).GetField("Dict")!)
    });
}
}