
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record NullableByteArrayWrap
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "NullableByteArrayWrap",
            typeof(Serde.Test.JsonSerializerTests.NullableByteArrayWrap).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("bytes", global::Serde.SerdeInfoProvider.GetSerializeInfo<byte[]?, Serde.NullableRefProxy.Ser<byte[], global::Serde.ByteArrayProxy>>())
            }
        );
    }
}
