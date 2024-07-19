
#nullable enable
namespace Serde.Test;
partial record AllInOne
{
    partial struct ColorEnumWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "ColorEnum",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("red", global::Serde.SerdeInfoProvider.GetInfo<AllInOne.ColorEnumWrap>(), typeof(Serde.Test.AllInOne.ColorEnum).GetField("Red")!),
("blue", global::Serde.SerdeInfoProvider.GetInfo<AllInOne.ColorEnumWrap>(), typeof(Serde.Test.AllInOne.ColorEnum).GetField("Blue")!),
("green", global::Serde.SerdeInfoProvider.GetInfo<AllInOne.ColorEnumWrap>(), typeof(Serde.Test.AllInOne.ColorEnum).GetField("Green")!)
    });
}
}