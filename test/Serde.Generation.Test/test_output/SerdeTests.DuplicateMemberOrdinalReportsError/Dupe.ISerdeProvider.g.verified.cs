//HintName: Dupe.ISerdeProvider.g.cs
partial record Dupe : Serde.ISerdeProvider<Dupe, Dupe._SerdeObj, Dupe>
{
    static Dupe._SerdeObj global::Serde.ISerdeProvider<Dupe, Dupe._SerdeObj, Dupe>.Instance { get; }
        = new Dupe._SerdeObj();
}
