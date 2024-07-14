//HintName: Serde.Test.AllInOne.ColorEnumSerdeInfo.cs
namespace Serde.Test;
partial record AllInOne
{
    internal static class ColorEnumSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ColorEnum",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Serde.Test.AllInOne.ColorEnum).GetField("Red")!),
("blue", typeof(Serde.Test.AllInOne.ColorEnum).GetField("Blue")!),
("green", typeof(Serde.Test.AllInOne.ColorEnum).GetField("Green")!)
    });
}
}