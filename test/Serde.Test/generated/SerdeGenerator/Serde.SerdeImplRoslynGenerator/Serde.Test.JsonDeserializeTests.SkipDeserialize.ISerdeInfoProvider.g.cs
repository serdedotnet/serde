
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record SkipDeserialize
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "SkipDeserialize",
            typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("required", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("skip", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>())
                {
                    MemberInfo = typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetProperty("Skip"),
                }
            }
        );
    }
}
