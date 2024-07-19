
#nullable enable
namespace Serde.Test;
partial class XmlTests
{
    partial class NestedArrays : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "NestedArrays",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("A", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<int[][],Serde.ArrayWrap.SerializeImpl<int[],Serde.ArrayWrap.SerializeImpl<int,global::Serde.Int32Wrap>>>>(), typeof(Serde.Test.XmlTests.NestedArrays).GetField("A")!)
    });
}
}