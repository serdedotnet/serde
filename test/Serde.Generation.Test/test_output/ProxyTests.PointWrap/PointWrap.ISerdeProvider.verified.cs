//HintName: PointWrap.ISerdeProvider.cs
partial struct PointWrap : Serde.ISerdeProvider<PointWrap, PointWrap._SerdeObj, Point>
{
    static PointWrap._SerdeObj global::Serde.ISerdeProvider<PointWrap, PointWrap._SerdeObj, Point>.Instance { get; }
        = new PointWrap._SerdeObj();
}
