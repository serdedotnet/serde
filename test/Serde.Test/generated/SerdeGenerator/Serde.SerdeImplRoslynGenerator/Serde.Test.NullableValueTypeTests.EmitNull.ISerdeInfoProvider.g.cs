
#nullable enable

namespace Serde.Test;

partial class NullableValueTypeTests
{
    partial record EmitNull
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "EmitNull",
            typeof(Serde.Test.NullableValueTypeTests.EmitNull).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("value", global::Serde.SerdeInfoProvider.GetSerializeInfo<int?, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>())
                {
                    MemberInfo = typeof(Serde.Test.NullableValueTypeTests.EmitNull).GetProperty("Value"),
                }
            }
        );
    }
}
