
#nullable enable

namespace Serde.Test;

partial class CustomImplTests
{
    partial record RgbWithFieldMap
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "RgbWithFieldMap",
        typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("red", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetField("Red")),
            ("green", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetField("Green")),
            ("blue", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetField("Blue"))
        }
        );
    }
}
