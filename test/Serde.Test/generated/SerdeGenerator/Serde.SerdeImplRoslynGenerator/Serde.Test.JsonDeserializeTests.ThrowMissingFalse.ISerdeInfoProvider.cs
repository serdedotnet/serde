
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record ThrowMissingFalse
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ThrowMissingFalse",
        typeof(Serde.Test.JsonDeserializeTests.ThrowMissingFalse).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("present", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.ThrowMissingFalse).GetProperty("Present")),
            ("missing", global::Serde.SerdeInfoProvider.GetDeserializeInfo<bool, global::Serde.BoolProxy>(), typeof(Serde.Test.JsonDeserializeTests.ThrowMissingFalse).GetProperty("Missing"))
        }
        );
    }
}
