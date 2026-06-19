
#nullable enable

namespace Serde.Test;

partial class MemberOrdinalTests
{
    partial record NoOrdinals
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "NoOrdinals",
        typeof(Serde.Test.MemberOrdinalTests.NoOrdinals).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.NoOrdinals).GetProperty("A")),
            ("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.MemberOrdinalTests.NoOrdinals).GetProperty("B"))
        }
        );
    }
}
