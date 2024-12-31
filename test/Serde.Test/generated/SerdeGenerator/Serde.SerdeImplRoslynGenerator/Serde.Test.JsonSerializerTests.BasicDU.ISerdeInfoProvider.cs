
#nullable enable
namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeUnion(
            "BasicDU",
            typeof(Serde.Test.JsonSerializerTests.BasicDU).GetCustomAttributesData(),
            System.Collections.Immutable.ImmutableArray.Create<global::Serde.ISerdeInfo>(
                        global::Serde.SerdeInfoProvider.GetInfo<_m_AProxy>(),
                        global::Serde.SerdeInfoProvider.GetInfo<_m_BProxy>()
            )
        );

        [global::Serde.GenerateSerialize]
        private sealed partial class _m_AProxy {}
        [global::Serde.GenerateSerialize]
        private sealed partial class _m_BProxy {}

    }
}