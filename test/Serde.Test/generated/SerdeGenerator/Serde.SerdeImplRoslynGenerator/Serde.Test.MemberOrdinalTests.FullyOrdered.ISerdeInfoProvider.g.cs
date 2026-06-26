
#nullable enable

namespace Serde.Test;

partial class MemberOrdinalTests
{
    partial record FullyOrdered
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "FullyOrdered",
            typeof(Serde.Test.MemberOrdinalTests.FullyOrdered).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.MemberOrdinalTests.FullyOrdered).GetProperty("B"),
                    Ordinal = 0,
                },
                new("D", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.MemberOrdinalTests.FullyOrdered).GetProperty("D"),
                    Ordinal = 1,
                },
                new("C", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.MemberOrdinalTests.FullyOrdered).GetProperty("C"),
                    Ordinal = 2,
                },
                new("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.MemberOrdinalTests.FullyOrdered).GetProperty("A"),
                    Ordinal = 3,
                }
            }
        );
    }
}
