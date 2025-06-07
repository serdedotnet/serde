//HintName: Some.Nested.Namespace.Base.ISerdeInfoProvider.g.cs

#nullable enable

namespace Some.Nested.Namespace;

partial record Base
{
    private static global::Serde.ISerdeInfo s_serdeInfo { get; } = Serde.SerdeInfo.MakeUnion(
        "Base",
        typeof(Some.Nested.Namespace.Base).GetCustomAttributesData(),
        System.Collections.Immutable.ImmutableArray.Create<global::Serde.ISerdeInfo>(
            global::Serde.SerdeInfoProvider.GetDeserializeInfo<Some.Nested.Namespace.Base.A, _m_AProxy>(),
            global::Serde.SerdeInfoProvider.GetDeserializeInfo<Some.Nested.Namespace.Base.B, _m_BProxy>()
        )
    );

    [global::Serde.GenerateDeserialize]
    private sealed partial class _m_AProxy {}
    [global::Serde.GenerateDeserialize]
    private sealed partial class _m_BProxy {}

}
