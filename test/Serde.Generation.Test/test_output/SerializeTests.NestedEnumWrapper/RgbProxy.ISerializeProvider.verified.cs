//HintName: RgbProxy.ISerializeProvider.cs
partial class RgbProxy : Serde.ISerializeProvider<Rgb>
{
    static global::Serde.ISerialize<Rgb> global::Serde.ISerializeProvider<Rgb>.Instance { get; }
        = new RgbProxy();
}
