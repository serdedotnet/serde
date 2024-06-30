namespace Serde.Test;
partial class JsonSerializerTests
{
    internal static class ColorSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "Color",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Serde.Test.JsonSerializerTests.Color).GetField("Red")!),
("green", typeof(Serde.Test.JsonSerializerTests.Color).GetField("Green")!),
("blue", typeof(Serde.Test.JsonSerializerTests.Color).GetField("Blue")!)
    });
}
}