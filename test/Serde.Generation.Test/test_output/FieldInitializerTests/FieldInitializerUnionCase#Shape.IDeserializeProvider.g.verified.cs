//HintName: Shape.IDeserializeProvider.g.cs
partial record Shape : Serde.IDeserializeProvider<Shape>
{
    static global::Serde.IDeserialize<Shape> global::Serde.IDeserializeProvider<Shape>.Instance { get; }
        = new Shape._DeObj();
}
