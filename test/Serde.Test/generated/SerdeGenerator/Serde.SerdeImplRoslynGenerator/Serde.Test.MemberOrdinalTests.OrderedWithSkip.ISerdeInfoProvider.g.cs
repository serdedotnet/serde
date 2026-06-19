
#nullable enable

namespace Serde.Test;

partial class MemberOrdinalTests
{
    partial record OrderedWithSkip
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "OrderedWithSkip",
        typeof(Serde.Test.MemberOrdinalTests.OrderedWithSkip).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.OrderedWithSkip).GetProperty("B")),
            ("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.OrderedWithSkip).GetProperty("A"))
        },
        new int[] { 0, 1 }
        );
    }
}
