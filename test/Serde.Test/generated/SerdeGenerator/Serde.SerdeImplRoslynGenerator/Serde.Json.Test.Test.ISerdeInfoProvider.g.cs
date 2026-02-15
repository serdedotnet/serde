
#nullable enable

namespace Serde.Json.Test;

partial class Test
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Test",
    typeof(Serde.Json.Test.Test).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("v2", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy>(), typeof(Serde.Json.Test.Test).GetField("v2")),
        ("vertices", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Numerics.Vector2[][], Serde.ArrayProxy.Ser<System.Numerics.Vector2[], Serde.ArrayProxy.Ser<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy>>>(), typeof(Serde.Json.Test.Test).GetField("vertices")),
        ("weights", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Numerics.Vector2[][], Serde.ArrayProxy.Ser<System.Numerics.Vector2[], Serde.ArrayProxy.Ser<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy2>>>(), typeof(Serde.Json.Test.Test).GetField("weights")),
        ("points", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Generic.Dictionary<System.Numerics.Vector3, System.Numerics.Vector2[][]>, Serde.DictProxy.Ser<System.Numerics.Vector3, System.Numerics.Vector2[][], Serde.Json.Test.Vector3Proxy, Serde.ArrayProxy.Ser<System.Numerics.Vector2[], Serde.ArrayProxy.Ser<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy>>>>(), typeof(Serde.Json.Test.Test).GetField("points"))
    }
    );
}
