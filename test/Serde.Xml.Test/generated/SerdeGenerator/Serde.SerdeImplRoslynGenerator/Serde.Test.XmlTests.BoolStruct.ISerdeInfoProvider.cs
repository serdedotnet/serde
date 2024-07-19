
#nullable enable
namespace Serde.Test;
partial class XmlTests
{
    partial struct BoolStruct : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "BoolStruct",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("BoolField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.BoolWrap>(), typeof(Serde.Test.XmlTests.BoolStruct).GetField("BoolField")!)
    });
}
}