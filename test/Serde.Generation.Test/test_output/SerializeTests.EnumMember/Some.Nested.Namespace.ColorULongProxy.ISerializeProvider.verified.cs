//HintName: Some.Nested.Namespace.ColorULongProxy.ISerializeProvider.cs

namespace Some.Nested.Namespace;

partial class ColorULongProxy : Serde.ISerializeProvider<Some.Nested.Namespace.ColorULong>
{
    static global::Serde.ISerialize<Some.Nested.Namespace.ColorULong> global::Serde.ISerializeProvider<Some.Nested.Namespace.ColorULong>.Instance { get; }
        = new Some.Nested.Namespace.ColorULongProxy();
}
