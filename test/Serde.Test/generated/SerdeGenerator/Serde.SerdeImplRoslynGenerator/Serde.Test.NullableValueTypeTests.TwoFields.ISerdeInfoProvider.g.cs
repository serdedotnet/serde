
#nullable enable

namespace Serde.Test;

partial class NullableValueTypeTests
{
    partial record TwoFields
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "TwoFields",
            typeof(Serde.Test.NullableValueTypeTests.TwoFields).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("first", global::Serde.SerdeInfoProvider.GetSerializeInfo<int?, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>()),
                new("second", global::Serde.SerdeInfoProvider.GetSerializeInfo<int?, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>())
            }
        );
    }
}
