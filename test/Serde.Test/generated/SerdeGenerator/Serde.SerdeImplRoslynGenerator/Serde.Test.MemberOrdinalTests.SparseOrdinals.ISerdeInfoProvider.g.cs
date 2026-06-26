
#nullable enable

namespace Serde.Test;

partial class MemberOrdinalTests
{
    partial record SparseOrdinals
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "SparseOrdinals",
            typeof(Serde.Test.MemberOrdinalTests.SparseOrdinals).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.MemberOrdinalTests.SparseOrdinals).GetProperty("B"),
                    Ordinal = 0,
                },
                new("C", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.MemberOrdinalTests.SparseOrdinals).GetProperty("C"),
                    Ordinal = 2,
                },
                new("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.MemberOrdinalTests.SparseOrdinals).GetProperty("A"),
                    Ordinal = 5,
                }
            }
        );
    }
}
