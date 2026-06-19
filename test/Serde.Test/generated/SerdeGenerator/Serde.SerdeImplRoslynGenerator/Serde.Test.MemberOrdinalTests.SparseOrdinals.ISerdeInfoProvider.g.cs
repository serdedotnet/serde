
#nullable enable

namespace Serde.Test;

partial class MemberOrdinalTests
{
    partial record SparseOrdinals
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "SparseOrdinals",
        typeof(Serde.Test.MemberOrdinalTests.SparseOrdinals).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.SparseOrdinals).GetProperty("B")),
            ("C", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.SparseOrdinals).GetProperty("C")),
            ("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.SparseOrdinals).GetProperty("A"))
        },
        new int[] { 0, 2, 5 }
        );
    }
}
