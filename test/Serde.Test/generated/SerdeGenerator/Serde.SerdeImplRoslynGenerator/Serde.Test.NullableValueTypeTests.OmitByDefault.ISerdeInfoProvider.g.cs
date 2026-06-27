
#nullable enable

namespace Serde.Test;

partial class NullableValueTypeTests
{
    partial record OmitByDefault
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "OmitByDefault",
            typeof(Serde.Test.NullableValueTypeTests.OmitByDefault).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("value", global::Serde.SerdeInfoProvider.GetSerializeInfo<int?, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>())
            }
        );
    }
}
