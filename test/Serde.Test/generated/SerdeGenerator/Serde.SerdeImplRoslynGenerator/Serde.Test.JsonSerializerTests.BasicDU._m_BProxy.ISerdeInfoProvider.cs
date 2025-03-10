
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU
    {
        partial class _m_BProxy : Serde.ISerdeInfoProvider
        {
            static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
                "B",
                typeof(Serde.Test.JsonSerializerTests.BasicDU.B).GetCustomAttributesData(),
                new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
                    ("y", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringProxy>(), typeof(Serde.Test.JsonSerializerTests.BasicDU.B).GetProperty("Y"))
                }
            );
        }
    }
}