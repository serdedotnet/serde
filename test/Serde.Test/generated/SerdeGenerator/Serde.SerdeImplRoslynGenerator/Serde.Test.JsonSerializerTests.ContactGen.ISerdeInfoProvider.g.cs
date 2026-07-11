
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record ContactGen
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ContactGen",
            typeof(Serde.Test.JsonSerializerTests.ContactGen).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("id", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
                new("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>()),
                new("email", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>())
            }
        );
    }
}
