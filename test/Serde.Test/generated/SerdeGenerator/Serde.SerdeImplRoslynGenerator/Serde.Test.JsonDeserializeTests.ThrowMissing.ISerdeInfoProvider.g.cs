
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct ThrowMissing
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ThrowMissing",
        typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("present", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetProperty("Present")),
            ("missing", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(), typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetProperty("Missing"))
        }
        );
    }
}
