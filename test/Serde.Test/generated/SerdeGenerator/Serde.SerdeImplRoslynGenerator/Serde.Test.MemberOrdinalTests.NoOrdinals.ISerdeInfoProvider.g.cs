
#nullable enable

namespace Serde.Test;

partial class MemberOrdinalTests
{
    partial record NoOrdinals
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "NoOrdinals",
            typeof(Serde.Test.MemberOrdinalTests.NoOrdinals).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
                new("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
