
#nullable enable
namespace Serde.Test;
partial class SerdeInfoTests
{
    partial record RgbProxy : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "Rgb",
        typeof(Serde.Test.SerdeInfoTests.Rgb).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("r", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.ByteWrap>(), typeof(Serde.Test.SerdeInfoTests.Rgb).GetField("R")!),
("g", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.ByteWrap>(), typeof(Serde.Test.SerdeInfoTests.Rgb).GetField("G")!),
("b", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.ByteWrap>(), typeof(Serde.Test.SerdeInfoTests.Rgb).GetField("B")!)
    });
}
}