
#nullable enable

namespace Serde.Json.Test;

partial class Vector2Proxy
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Vector2",
    typeof(System.Numerics.Vector2).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("X", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>(), typeof(System.Numerics.Vector2).GetField("X")),
        ("Y", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>(), typeof(System.Numerics.Vector2).GetField("Y"))
    }
    );
}
