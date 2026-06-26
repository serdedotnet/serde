
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial class NullableFields
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "NullableFields",
            typeof(Serde.Test.JsonDeserializeTests.NullableFields).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("s", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>()),
                new("dict", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Generic.Dictionary<string, string?>, Serde.DictProxy.De<string, string?, global::Serde.StringProxy, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>>())
            }
        );
    }
}
