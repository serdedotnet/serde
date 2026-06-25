
#nullable enable

namespace Serde.Test;

partial class MemberOrdinalTests
{
    partial record FullyOrdered
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "FullyOrdered",
        typeof(Serde.Test.MemberOrdinalTests.FullyOrdered).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.FullyOrdered).GetProperty("B")),
            ("D", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.FullyOrdered).GetProperty("D")),
            ("C", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.FullyOrdered).GetProperty("C")),
            ("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.FullyOrdered).GetProperty("A"))
        },
        new int[] { 0, 1, 2, 3 }
        );
    }
}
