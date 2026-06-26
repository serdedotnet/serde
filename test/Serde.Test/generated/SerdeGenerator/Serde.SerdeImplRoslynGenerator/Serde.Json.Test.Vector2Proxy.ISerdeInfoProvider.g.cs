
#nullable enable

namespace Serde.Json.Test;

partial class Vector2Proxy
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Vector2",
        typeof(System.Numerics.Vector2).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("x", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>()),
            new("y", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>())
        }
    );
}
