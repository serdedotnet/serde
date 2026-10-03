
#nullable enable

namespace Serde.Test;

partial class SerializeNullTests
{
    partial class MemberOption
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "MemberOption",
            typeof(Serde.Test.SerializeNullTests.MemberOption).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("value", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>())
                {
                    MemberInfo = typeof(Serde.Test.SerializeNullTests.MemberOption).GetProperty("Value"),
                    IsOptional = true,
                }
            }
        );
    }
}
