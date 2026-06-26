
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct SetToNull
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "SetToNull",
            typeof(Serde.Test.JsonDeserializeTests.SetToNull).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("present", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("missing", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>())
            }
        );
    }
}
