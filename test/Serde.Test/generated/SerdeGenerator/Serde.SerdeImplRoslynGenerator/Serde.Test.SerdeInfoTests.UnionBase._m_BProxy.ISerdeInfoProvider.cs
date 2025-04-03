
#nullable enable

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record UnionBase
    {
        partial class _m_BProxy
        {
            private static readonly global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
                "B",
            typeof(Serde.Test.SerdeInfoTests.UnionBase.B).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {

            }
            );
        }
    }
}