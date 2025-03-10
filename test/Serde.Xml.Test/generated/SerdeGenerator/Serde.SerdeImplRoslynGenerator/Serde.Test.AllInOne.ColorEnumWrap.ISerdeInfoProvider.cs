
#nullable enable
namespace Serde.Test;
partial record AllInOne
{
    partial struct ColorEnumWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorEnum",
        typeof(Serde.Test.AllInOne.ColorEnum).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(),
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Serde.Test.AllInOne.ColorEnum).GetField("Red")),
("blue", typeof(Serde.Test.AllInOne.ColorEnum).GetField("Blue")),
("green", typeof(Serde.Test.AllInOne.ColorEnum).GetField("Green"))
    });
}
}