
#nullable enable

namespace Serde.Test;

partial class CustomImplTests
{
    partial record RgbWithFieldMap
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "RgbWithFieldMap",
            typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("red", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
                new("green", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
                new("blue", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
