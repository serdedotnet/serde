
#nullable enable

namespace Serde.Test;

partial class NullableValueTypeTests
{
    partial record Outer
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Outer",
            typeof(Serde.Test.NullableValueTypeTests.Outer).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("inner", global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.NullableValueTypeTests.OmitByDefault, Serde.Test.NullableValueTypeTests.OmitByDefault>()),
                new("trailing", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
