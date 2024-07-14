namespace Serde.Test;
partial class JsonSerializerTests
{
    internal static class ColorSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "Color",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Serde.Test.JsonSerializerTests.Color).GetField("Red")!),
("green", typeof(Serde.Test.JsonSerializerTests.Color).GetField("Green")!),
("blue", typeof(Serde.Test.JsonSerializerTests.Color).GetField("Blue")!)
    });
}
}