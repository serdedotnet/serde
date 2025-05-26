//HintName: Rgb.IDeserializeProvider.cs
partial struct Rgb : Serde.IDeserializeProvider<Rgb>
{
    static global::Serde.IDeserialize<Rgb> global::Serde.IDeserializeProvider<Rgb>.Instance { get; }
        = new Rgb._DeObj();
}
