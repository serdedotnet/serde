
#nullable enable

namespace Serde.Test;

partial class RoundtripTests
{
    partial record Point
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Point",
            typeof(Serde.Test.RoundtripTests.Point).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("X", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
                new("Y", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
