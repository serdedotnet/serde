//HintName: Shape.ISerdeInfoProvider.g.cs

#nullable enable
partial record Shape
{
    private static global::Serde.ISerdeInfo s_serdeInfo { get; } = Serde.SerdeInfo.MakeUnion(
        "Shape",
        typeof(Shape).GetCustomAttributesData(),
        System.Collections.Immutable.ImmutableArray.Create<global::Serde.ISerdeInfo>(
            global::Serde.SerdeInfoProvider.GetDeserializeInfo<Shape.Install, _m_InstallProxy>()
        )
    );

    [global::Serde.GenerateDeserialize]
    private sealed partial class _m_InstallProxy {}

}
