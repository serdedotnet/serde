
#nullable enable

namespace Serde.Json.Test;

partial class Vector4Proxy
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Vector4",
        typeof(System.Numerics.Vector4).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("x", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>()),
            new("y", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>()),
            new("z", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>()),
            new("w", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>())
        }
    );
}
