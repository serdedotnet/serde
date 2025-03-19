
#nullable enable

namespace Serde.Test;

partial class SampleTest
{
    partial record Address
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Address",
        typeof(Serde.Test.SampleTest.Address).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("Name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.SampleTest.Address).GetField("Name")),
            ("Line1", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.SampleTest.Address).GetField("Line1")),
            ("City", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>(), typeof(Serde.Test.SampleTest.Address).GetField("City")),
            ("State", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.SampleTest.Address).GetField("State")),
            ("Zip", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.SampleTest.Address).GetField("Zip"))
        }
        );
    }
}