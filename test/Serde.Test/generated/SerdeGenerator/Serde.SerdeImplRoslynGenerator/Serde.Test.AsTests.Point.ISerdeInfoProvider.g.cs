
#nullable enable

namespace Serde.Test;

partial class AsTests
{
    partial record Point
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Point",
        typeof(Serde.Test.AsTests.Point).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("x", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.AsTests.Point).GetProperty("X")),
            ("y", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.AsTests.Point).GetProperty("Y"))
        }
        );
    }
}
