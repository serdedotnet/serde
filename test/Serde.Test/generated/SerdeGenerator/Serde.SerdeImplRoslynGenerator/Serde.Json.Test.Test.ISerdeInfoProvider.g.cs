
#nullable enable

namespace Serde.Json.Test;

partial class Test
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Test",
        typeof(Serde.Json.Test.Test).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("v2", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy>()),
            new("vertices", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Numerics.Vector2[][], Serde.ArrayProxy.Ser<System.Numerics.Vector2[], Serde.ArrayProxy.Ser<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy>>>()),
            new("weights", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Numerics.Vector2[][], Serde.ArrayProxy.Ser<System.Numerics.Vector2[], Serde.ArrayProxy.Ser<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy2>>>())
            {
                MemberInfo = typeof(Serde.Json.Test.Test).GetField("weights"),
            },
            new("points", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Generic.Dictionary<System.Numerics.Vector3, System.Numerics.Vector2[][]>, Serde.DictProxy.Ser<System.Numerics.Vector3, System.Numerics.Vector2[][], Serde.Json.Test.Vector3Proxy, Serde.ArrayProxy.Ser<System.Numerics.Vector2[], Serde.ArrayProxy.Ser<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy>>>>())
            {
                MemberInfo = typeof(Serde.Json.Test.Test).GetField("points"),
            }
        }
    );
}
