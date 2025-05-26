
#nullable enable

namespace Serde.Json.Test;

partial class Poco
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Poco",
    typeof(Serde.Json.Test.Poco).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("id", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Json.Test.Poco).GetProperty("Id"))
    }
    );
}
