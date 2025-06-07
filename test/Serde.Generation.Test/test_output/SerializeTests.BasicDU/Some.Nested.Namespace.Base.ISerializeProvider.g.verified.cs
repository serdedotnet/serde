//HintName: Some.Nested.Namespace.Base.ISerializeProvider.g.cs

namespace Some.Nested.Namespace;

partial record Base : Serde.ISerializeProvider<Some.Nested.Namespace.Base>
{
    static global::Serde.ISerialize<Some.Nested.Namespace.Base> global::Serde.ISerializeProvider<Some.Nested.Namespace.Base>.Instance { get; }
        = new Some.Nested.Namespace.Base._SerObj();
}
