
#nullable enable

namespace Serde.Test;

partial class MemberOrdinalTests
{
    partial record Reordered
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Reordered",
            typeof(Serde.Test.MemberOrdinalTests.Reordered).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.MemberOrdinalTests.Reordered).GetProperty("B"),
                    Ordinal = 0,
                },
                new("C", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.MemberOrdinalTests.Reordered).GetProperty("C"),
                    Ordinal = 1,
                },
                new("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.MemberOrdinalTests.Reordered).GetProperty("A"),
                    Ordinal = 2,
                }
            }
        );
    }
}
