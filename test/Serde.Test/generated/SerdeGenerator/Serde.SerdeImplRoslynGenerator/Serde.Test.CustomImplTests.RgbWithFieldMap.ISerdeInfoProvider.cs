
#nullable enable
namespace Serde.Test;
partial class CustomImplTests
{
    partial record RgbWithFieldMap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "RgbWithFieldMap",
        typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("red", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetField("Red")!),
("green", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetField("Green")!),
("blue", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetField("Blue")!)
    });
}
}