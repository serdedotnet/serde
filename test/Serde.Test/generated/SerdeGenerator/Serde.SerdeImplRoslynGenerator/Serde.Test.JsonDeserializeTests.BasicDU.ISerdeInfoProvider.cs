
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU
    {
        private static global::Serde.ISerdeInfo s_serdeInfo { get; } = Serde.SerdeInfo.MakeUnion(
            "BasicDU",
            typeof(Serde.Test.JsonDeserializeTests.BasicDU).GetCustomAttributesData(),
            System.Collections.Immutable.ImmutableArray.Create<global::Serde.ISerdeInfo>(
                global::Serde.SerdeInfoProvider.GetDeserializeInfo<Serde.Test.JsonDeserializeTests.BasicDU.A, _m_AProxy>(),
                global::Serde.SerdeInfoProvider.GetDeserializeInfo<Serde.Test.JsonDeserializeTests.BasicDU.B, _m_BProxy>()
            )
        );

        [global::Serde.GenerateDeserialize]
        private sealed partial class _m_AProxy {}
        [global::Serde.GenerateDeserialize]
        private sealed partial class _m_BProxy {}

    }
}
