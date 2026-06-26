
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial struct Color
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Color",
            typeof(Serde.Test.JsonSerializerTests.Color).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("red", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
                new("green", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
                new("blue", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
