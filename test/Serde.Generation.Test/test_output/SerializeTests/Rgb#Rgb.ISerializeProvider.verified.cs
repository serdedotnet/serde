//HintName: Rgb.ISerializeProvider.cs
partial struct Rgb : Serde.ISerializeProvider<Rgb>
{
    static global::Serde.ISerialize<Rgb> global::Serde.ISerializeProvider<Rgb>.Instance { get; }
        = new Rgb._SerObj();
}
