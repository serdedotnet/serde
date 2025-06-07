//HintName: Some.Nested.Namespace.Base._m_AProxy.ISerializeProvider.g.cs

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_AProxy : Serde.ISerializeProvider<Some.Nested.Namespace.Base.A>
    {
        static global::Serde.ISerialize<Some.Nested.Namespace.Base.A> global::Serde.ISerializeProvider<Some.Nested.Namespace.Base.A>.Instance { get; }
            = new Some.Nested.Namespace.Base._m_AProxy._SerObj();
    }
}
