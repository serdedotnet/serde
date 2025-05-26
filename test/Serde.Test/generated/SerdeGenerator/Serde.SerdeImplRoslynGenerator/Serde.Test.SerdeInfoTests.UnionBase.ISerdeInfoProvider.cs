
#nullable enable

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record UnionBase
    {
        private static global::Serde.ISerdeInfo s_serdeInfo { get; } = Serde.SerdeInfo.MakeUnion(
            "UnionBase",
            typeof(Serde.Test.SerdeInfoTests.UnionBase).GetCustomAttributesData(),
            System.Collections.Immutable.ImmutableArray.Create<global::Serde.ISerdeInfo>(
                global::Serde.SerdeInfoProvider.GetDeserializeInfo<Serde.Test.SerdeInfoTests.UnionBase.A, _m_AProxy>(),
                global::Serde.SerdeInfoProvider.GetDeserializeInfo<Serde.Test.SerdeInfoTests.UnionBase.B, _m_BProxy>()
            )
        );

        [global::Serde.GenerateDeserialize]
        private sealed partial class _m_AProxy {}
        [global::Serde.GenerateDeserialize]
        private sealed partial class _m_BProxy {}

    }
}
