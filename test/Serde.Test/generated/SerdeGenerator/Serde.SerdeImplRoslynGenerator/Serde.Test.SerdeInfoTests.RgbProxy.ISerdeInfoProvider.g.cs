
#nullable enable

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record RgbProxy
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Rgb",
            typeof(Serde.Test.SerdeInfoTests.Rgb).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("r", global::Serde.SerdeInfoProvider.GetDeserializeInfo<byte, global::Serde.U8Proxy>()),
                new("g", global::Serde.SerdeInfoProvider.GetDeserializeInfo<byte, global::Serde.U8Proxy>()),
                new("b", global::Serde.SerdeInfoProvider.GetDeserializeInfo<byte, global::Serde.U8Proxy>())
            }
        );
    }
}
