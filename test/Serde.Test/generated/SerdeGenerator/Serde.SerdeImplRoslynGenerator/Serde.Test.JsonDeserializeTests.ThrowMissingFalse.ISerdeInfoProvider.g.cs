
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record ThrowMissingFalse
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ThrowMissingFalse",
            typeof(Serde.Test.JsonDeserializeTests.ThrowMissingFalse).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("present", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("missing", global::Serde.SerdeInfoProvider.GetDeserializeInfo<bool, global::Serde.BoolProxy>())
                {
                    MemberInfo = typeof(Serde.Test.JsonDeserializeTests.ThrowMissingFalse).GetProperty("Missing"),
                }
            }
        );
    }
}
