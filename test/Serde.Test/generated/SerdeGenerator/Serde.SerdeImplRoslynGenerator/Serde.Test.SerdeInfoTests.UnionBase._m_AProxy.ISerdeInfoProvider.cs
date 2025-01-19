
#nullable enable

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record UnionBase
    {
        partial class _m_AProxy : Serde.ISerdeInfoProvider
        {
            static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
                "A",
                typeof(Serde.Test.SerdeInfoTests.UnionBase.A).GetCustomAttributesData(),
                new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {

                }
            );
        }
    }
}