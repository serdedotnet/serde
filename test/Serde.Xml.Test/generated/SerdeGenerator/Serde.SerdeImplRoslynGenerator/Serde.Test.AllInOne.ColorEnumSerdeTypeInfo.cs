namespace Serde.Test;
partial record AllInOne
{
    internal static class ColorEnumSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "ColorEnum",
        Serde.TypeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Serde.Test.AllInOne.ColorEnum).GetField("Red")!),
("blue", typeof(Serde.Test.AllInOne.ColorEnum).GetField("Blue")!),
("green", typeof(Serde.Test.AllInOne.ColorEnum).GetField("Green")!)
    });
}
}