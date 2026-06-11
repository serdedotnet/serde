//HintName: Other.ColorProxy.IDeserializeProvider.g.cs

namespace Other;

partial class ColorProxy : Serde.IDeserializeProvider<Other.Color>
{
    static global::Serde.IDeserialize<Other.Color> global::Serde.IDeserializeProvider<Other.Color>.Instance { get; }
        = new Other.ColorProxy();
}
