
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record SkipDeserialize
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "SkipDeserialize",
        typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("required", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetProperty("Required")),
            ("skip", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetProperty("Skip"))
        }
        );
    }
}
