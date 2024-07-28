
#nullable enable
namespace Serde.Test;
partial class XmlTests
{
    partial struct BoolStruct : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "BoolStruct",
        typeof(Serde.Test.XmlTests.BoolStruct).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("BoolField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.BoolWrap>(), typeof(Serde.Test.XmlTests.BoolStruct).GetField("BoolField")!)
    });
}
}