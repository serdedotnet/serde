
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU
    {
        private static global::Serde.ISerdeInfo s_serdeInfo { get; } = Serde.SerdeInfo.MakeUnion(
            "BasicDU",
            typeof(Serde.Test.JsonSerializerTests.BasicDU).GetCustomAttributesData(),
            System.Collections.Immutable.ImmutableArray.Create<global::Serde.ISerdeInfo>(
                global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.JsonSerializerTests.BasicDU.A, _m_AProxy>(),
                global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.JsonSerializerTests.BasicDU.B, _m_BProxy>()
            )
        );

        [global::Serde.GenerateSerialize]
        private sealed partial class _m_AProxy {}
        [global::Serde.GenerateSerialize]
        private sealed partial class _m_BProxy {}

    }
}
