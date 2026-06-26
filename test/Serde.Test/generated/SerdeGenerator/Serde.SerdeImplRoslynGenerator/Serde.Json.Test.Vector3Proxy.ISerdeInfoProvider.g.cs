
#nullable enable

namespace Serde.Json.Test;

partial class Vector3Proxy
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Vector3",
        typeof(System.Numerics.Vector3).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("x", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>()),
            new("y", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>()),
            new("z", global::Serde.SerdeInfoProvider.GetSerializeInfo<float, global::Serde.F32Proxy>())
        }
    );
}
