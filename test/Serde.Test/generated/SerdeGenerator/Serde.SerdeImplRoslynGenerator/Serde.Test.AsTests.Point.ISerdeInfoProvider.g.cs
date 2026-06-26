
#nullable enable

namespace Serde.Test;

partial class AsTests
{
    partial record Point
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Point",
            typeof(Serde.Test.AsTests.Point).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("x", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
                new("y", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
