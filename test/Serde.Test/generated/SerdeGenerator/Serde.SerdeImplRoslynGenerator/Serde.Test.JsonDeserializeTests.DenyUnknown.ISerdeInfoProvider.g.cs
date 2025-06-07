
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct DenyUnknown
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "DenyUnknown",
        typeof(Serde.Test.JsonDeserializeTests.DenyUnknown).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("present", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.DenyUnknown).GetProperty("Present")),
            ("missing", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(), typeof(Serde.Test.JsonDeserializeTests.DenyUnknown).GetProperty("Missing"))
        }
        );
    }
}
