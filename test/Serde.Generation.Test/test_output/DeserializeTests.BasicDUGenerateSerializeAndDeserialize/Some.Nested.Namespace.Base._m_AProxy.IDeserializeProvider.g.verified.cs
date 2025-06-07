//HintName: Some.Nested.Namespace.Base._m_AProxy.IDeserializeProvider.g.cs

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_AProxy : Serde.IDeserializeProvider<Some.Nested.Namespace.Base.A>
    {
        static global::Serde.IDeserialize<Some.Nested.Namespace.Base.A> global::Serde.IDeserializeProvider<Some.Nested.Namespace.Base.A>.Instance { get; }
            = new Some.Nested.Namespace.Base._m_AProxy._DeObj();
    }
}
