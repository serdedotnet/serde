
#nullable enable

namespace Serde.Test;

partial class SampleTest
{
    partial record OrderedItem
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "OrderedItem",
        typeof(Serde.Test.SampleTest.OrderedItem).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("ItemName", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.SampleTest.OrderedItem).GetField("ItemName")),
            ("Description", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.SampleTest.OrderedItem).GetField("Description")),
            ("UnitPrice", global::Serde.SerdeInfoProvider.GetSerializeInfo<decimal, global::Serde.DecimalProxy>(), typeof(Serde.Test.SampleTest.OrderedItem).GetField("UnitPrice")),
            ("Quantity", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.SampleTest.OrderedItem).GetField("Quantity")),
            ("LineTotal", global::Serde.SerdeInfoProvider.GetSerializeInfo<decimal, global::Serde.DecimalProxy>(), typeof(Serde.Test.SampleTest.OrderedItem).GetField("LineTotal"))
        }
        );
    }
}