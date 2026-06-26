
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU
    {
        partial class _m_AProxy
        {
            private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
                "A",
                typeof(Serde.Test.JsonSerializerTests.BasicDU.A).GetCustomAttributesData(),
                new global::Serde.SerdeInfo.FieldInfo[] {
                    new("x", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                }
            );
        }
    }
}
