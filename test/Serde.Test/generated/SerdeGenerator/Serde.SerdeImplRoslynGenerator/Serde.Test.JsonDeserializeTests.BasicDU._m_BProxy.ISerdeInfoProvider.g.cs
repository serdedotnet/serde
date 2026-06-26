
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU
    {
        partial class _m_BProxy
        {
            private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
                "B",
                typeof(Serde.Test.JsonDeserializeTests.BasicDU.B).GetCustomAttributesData(),
                new global::Serde.SerdeInfo.FieldInfo[] {
                    new("y", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>())
                }
            );
        }
    }
}
