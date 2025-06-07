
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial struct Color
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Color",
        typeof(Serde.Test.JsonSerializerTests.Color).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("red", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.JsonSerializerTests.Color).GetField("Red")),
            ("green", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.JsonSerializerTests.Color).GetField("Green")),
            ("blue", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.JsonSerializerTests.Color).GetField("Blue"))
        }
        );
    }
}
