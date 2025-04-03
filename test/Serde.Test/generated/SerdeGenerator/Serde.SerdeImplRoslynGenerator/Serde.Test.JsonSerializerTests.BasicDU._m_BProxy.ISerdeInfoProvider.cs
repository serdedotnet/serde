
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU
    {
        partial class _m_BProxy
        {
            private static readonly global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
                "B",
            typeof(Serde.Test.JsonSerializerTests.BasicDU.B).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
                ("y", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.JsonSerializerTests.BasicDU.B).GetProperty("Y"))
            }
            );
        }
    }
}