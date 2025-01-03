//HintName: Some.Nested.Namespace.Base.ISerdeInfoProvider.cs

#nullable enable
namespace Some.Nested.Namespace;

partial record Base : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeUnion(
        "Base",
        typeof(Some.Nested.Namespace.Base).GetCustomAttributesData(),
        System.Collections.Immutable.ImmutableArray.Create<global::Serde.ISerdeInfo>(
                    global::Serde.SerdeInfoProvider.GetInfo<_m_AProxy>(),
                    global::Serde.SerdeInfoProvider.GetInfo<_m_BProxy>()
        )
    );

    [global::Serde.GenerateSerde]
    private sealed partial class _m_AProxy {}
    [global::Serde.GenerateSerde]
    private sealed partial class _m_BProxy {}

}