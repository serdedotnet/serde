
#nullable enable

namespace Serde.Test;

partial class MemberOrdinalTests
{
    partial record Reordered
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Reordered",
        typeof(Serde.Test.MemberOrdinalTests.Reordered).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.Reordered).GetProperty("B")),
            ("C", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.Reordered).GetProperty("C")),
            ("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.Reordered).GetProperty("A"))
        },
        new int[] { 0, 1, 2 }
        );
    }
}
