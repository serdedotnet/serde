//HintName: Some.Nested.Namespace.Base._m_BProxy.IDeserializeProvider.g.cs

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_BProxy : Serde.IDeserializeProvider<Some.Nested.Namespace.Base.B>
    {
        static global::Serde.IDeserialize<Some.Nested.Namespace.Base.B> global::Serde.IDeserializeProvider<Some.Nested.Namespace.Base.B>.Instance { get; }
            = new Some.Nested.Namespace.Base._m_BProxy._DeObj();
    }
}
