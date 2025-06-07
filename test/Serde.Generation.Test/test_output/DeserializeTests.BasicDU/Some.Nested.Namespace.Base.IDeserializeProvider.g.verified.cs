//HintName: Some.Nested.Namespace.Base.IDeserializeProvider.g.cs

namespace Some.Nested.Namespace;

partial record Base : Serde.IDeserializeProvider<Some.Nested.Namespace.Base>
{
    static global::Serde.IDeserialize<Some.Nested.Namespace.Base> global::Serde.IDeserializeProvider<Some.Nested.Namespace.Base>.Instance { get; }
        = new Some.Nested.Namespace.Base._DeObj();
}
