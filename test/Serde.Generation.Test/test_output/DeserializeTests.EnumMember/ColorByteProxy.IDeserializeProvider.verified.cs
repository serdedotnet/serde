//HintName: ColorByteProxy.IDeserializeProvider.cs
partial class ColorByteProxy : Serde.IDeserializeProvider<ColorByte>
{
    static global::Serde.IDeserialize<ColorByte> global::Serde.IDeserializeProvider<ColorByte>.Instance { get; }
        = new ColorByteProxy();
}
