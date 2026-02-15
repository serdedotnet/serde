
#nullable enable

namespace Serde.Json.Test;

partial class Vector2Proxy2
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Vector2",
    typeof(System.Numerics.Vector2).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("x", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>(), typeof(System.Numerics.Vector2).GetField("X")),
        ("y", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>(), typeof(System.Numerics.Vector2).GetField("Y"))
    }
    );
}
