
#nullable enable

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record UnionBase : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeUnion(
            "UnionBase",
            typeof(Serde.Test.SerdeInfoTests.UnionBase).GetCustomAttributesData(),
            System.Collections.Immutable.ImmutableArray.Create<global::Serde.ISerdeInfo>(
                        global::Serde.SerdeInfoProvider.GetInfo<_m_AProxy>(),
                        global::Serde.SerdeInfoProvider.GetInfo<_m_BProxy>()
            )
        );

        [global::Serde.GenerateDeserialize]
        private sealed partial class _m_AProxy {}
        [global::Serde.GenerateDeserialize]
        private sealed partial class _m_BProxy {}

    }
}