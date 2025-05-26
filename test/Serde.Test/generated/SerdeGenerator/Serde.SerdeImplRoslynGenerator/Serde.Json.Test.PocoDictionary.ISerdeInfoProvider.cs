
#nullable enable

namespace Serde.Json.Test;

partial class PocoDictionary
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "PocoDictionary",
    typeof(Serde.Json.Test.PocoDictionary).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("key", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Generic.Dictionary<string, string>, Serde.DictProxy.De<string, string, global::Serde.StringProxy, global::Serde.StringProxy>>(), typeof(Serde.Json.Test.PocoDictionary).GetProperty("key"))
    }
    );
}
