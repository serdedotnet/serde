
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU
    {
        partial class _m_AProxy
        {
            private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
                "A",
                typeof(Serde.Test.JsonDeserializeTests.BasicDU.A).GetCustomAttributesData(),
                new global::Serde.SerdeInfo.FieldInfo[] {
                    new("x", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
                }
            );
        }
    }
}
