
#nullable enable

namespace Serde.Test;

partial class MemberOrdinalTests
{
    partial record OrderedWithSkip
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "OrderedWithSkip",
            typeof(Serde.Test.MemberOrdinalTests.OrderedWithSkip).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.MemberOrdinalTests.OrderedWithSkip).GetProperty("B"),
                    Ordinal = 0,
                },
                new("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.MemberOrdinalTests.OrderedWithSkip).GetProperty("A"),
                    Ordinal = 1,
                }
            }
        );
    }
}
