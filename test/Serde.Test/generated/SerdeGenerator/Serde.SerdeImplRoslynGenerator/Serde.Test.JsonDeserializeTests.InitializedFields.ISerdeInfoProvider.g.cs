
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial class InitializedFields
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "InitializedFields",
            typeof(Serde.Test.JsonDeserializeTests.InitializedFields).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("num", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>()),
                new("str", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("required", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
