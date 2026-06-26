
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record Location
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Location",
            typeof(Serde.Test.JsonDeserializeTests.Location).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("id", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>()),
                new("address1", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("address2", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("city", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("state", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("postalCode", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("name", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("phoneNumber", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("country", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>())
            }
        );
    }
}
