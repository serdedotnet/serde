
#nullable enable

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record RgbProxy
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Rgb",
        typeof(Serde.Test.SerdeInfoTests.Rgb).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("r", global::Serde.SerdeInfoProvider.GetDeserializeInfo<byte, global::Serde.U8Proxy>(), typeof(Serde.Test.SerdeInfoTests.Rgb).GetField("R")),
            ("g", global::Serde.SerdeInfoProvider.GetDeserializeInfo<byte, global::Serde.U8Proxy>(), typeof(Serde.Test.SerdeInfoTests.Rgb).GetField("G")),
            ("b", global::Serde.SerdeInfoProvider.GetDeserializeInfo<byte, global::Serde.U8Proxy>(), typeof(Serde.Test.SerdeInfoTests.Rgb).GetField("B"))
        }
        );
    }
}
