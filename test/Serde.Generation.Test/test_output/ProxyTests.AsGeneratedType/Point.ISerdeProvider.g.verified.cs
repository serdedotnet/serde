//HintName: Point.ISerdeProvider.g.cs
partial record Point : Serde.ISerdeProvider<Point, Point._SerdeObj, Point>
{
    static Point._SerdeObj global::Serde.ISerdeProvider<Point, Point._SerdeObj, Point>.Instance { get; }
        = new Point._SerdeObj();
}
