
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct ThrowMissing
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ThrowMissing",
            typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("present", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("missing", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>())
                {
                    MemberInfo = typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetProperty("Missing"),
                }
            }
        );
    }
}
