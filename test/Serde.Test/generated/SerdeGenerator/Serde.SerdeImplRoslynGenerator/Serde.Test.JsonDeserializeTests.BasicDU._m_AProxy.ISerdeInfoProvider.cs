
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU
    {
        partial class _m_AProxy : Serde.ISerdeInfoProvider
        {
            static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
                "A",
                typeof(Serde.Test.JsonDeserializeTests.BasicDU.A).GetCustomAttributesData(),
                new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
                    ("x", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Proxy>(), typeof(Serde.Test.JsonDeserializeTests.BasicDU.A).GetProperty("X")!)
                }
            );
        }
    }
}