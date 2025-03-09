
#nullable enable

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record UnionBase
    {
        partial class _m_BProxy : Serde.ISerdeInfoProvider
        {
            static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
                "B",
                typeof(Serde.Test.SerdeInfoTests.UnionBase.B).GetCustomAttributesData(),
                new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {

                }
            );
        }
    }
}