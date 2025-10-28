
#nullable enable

namespace Serde.Test;

partial class RoundtripTests
{
    partial record Point
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Point",
        typeof(Serde.Test.RoundtripTests.Point).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("X", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.RoundtripTests.Point).GetProperty("X")),
            ("Y", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.RoundtripTests.Point).GetProperty("Y"))
        }
        );
    }
}
