
#nullable enable
namespace Serde.Test;
partial class SampleTest
{
    partial record OrderedItem : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "OrderedItem",
        typeof(Serde.Test.SampleTest.OrderedItem).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("ItemName", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.SampleTest.OrderedItem).GetField("ItemName")!),
("Description", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.SampleTest.OrderedItem).GetField("Description")!),
("UnitPrice", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.DecimalWrap>(), typeof(Serde.Test.SampleTest.OrderedItem).GetField("UnitPrice")!),
("Quantity", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Test.SampleTest.OrderedItem).GetField("Quantity")!),
("LineTotal", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.DecimalWrap>(), typeof(Serde.Test.SampleTest.OrderedItem).GetField("LineTotal")!)
    });
}
}