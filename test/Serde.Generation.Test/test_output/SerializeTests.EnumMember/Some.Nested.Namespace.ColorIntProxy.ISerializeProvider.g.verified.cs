//HintName: Some.Nested.Namespace.ColorIntProxy.ISerializeProvider.g.cs

namespace Some.Nested.Namespace;

partial class ColorIntProxy : Serde.ISerializeProvider<Some.Nested.Namespace.ColorInt>
{
    static global::Serde.ISerialize<Some.Nested.Namespace.ColorInt> global::Serde.ISerializeProvider<Some.Nested.Namespace.ColorInt>.Instance { get; }
        = new Some.Nested.Namespace.ColorIntProxy();
}
