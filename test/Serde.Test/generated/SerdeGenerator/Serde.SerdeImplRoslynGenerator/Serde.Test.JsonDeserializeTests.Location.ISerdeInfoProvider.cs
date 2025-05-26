
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record Location
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Location",
        typeof(Serde.Test.JsonDeserializeTests.Location).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("id", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.JsonDeserializeTests.Location).GetProperty("Id")),
            ("address1", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.Location).GetProperty("Address1")),
            ("address2", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.Location).GetProperty("Address2")),
            ("city", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.Location).GetProperty("City")),
            ("state", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.Location).GetProperty("State")),
            ("postalCode", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.Location).GetProperty("PostalCode")),
            ("name", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.Location).GetProperty("Name")),
            ("phoneNumber", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.Location).GetProperty("PhoneNumber")),
            ("country", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.Location).GetProperty("Country"))
        }
        );
    }
}
