
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record DUWithInitializer
    {
        private static global::Serde.ISerdeInfo s_serdeInfo { get; } = Serde.SerdeInfo.MakeUnion(
            "DUWithInitializer",
            typeof(Serde.Test.JsonDeserializeTests.DUWithInitializer).GetCustomAttributesData(),
            System.Collections.Immutable.ImmutableArray.Create<global::Serde.ISerdeInfo>(
                global::Serde.SerdeInfoProvider.GetDeserializeInfo<Serde.Test.JsonDeserializeTests.DUWithInitializer.A, _m_AProxy>()
            )
        );

        [global::Serde.GenerateDeserialize]
        private sealed partial class _m_AProxy {}

    }
}
