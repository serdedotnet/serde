
#nullable enable
namespace Serde.Test;
partial class XmlTests
{
    partial class NestedArrays : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "NestedArrays",
        typeof(Serde.Test.XmlTests.NestedArrays).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("A", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<int[][],Serde.ArrayWrap.SerializeImpl<int[],Serde.ArrayWrap.SerializeImpl<int,global::Serde.Int32Wrap>>>>(), typeof(Serde.Test.XmlTests.NestedArrays).GetField("A")!)
    });
}
}