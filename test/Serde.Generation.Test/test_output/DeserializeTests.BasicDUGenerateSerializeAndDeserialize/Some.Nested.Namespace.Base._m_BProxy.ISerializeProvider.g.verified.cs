//HintName: Some.Nested.Namespace.Base._m_BProxy.ISerializeProvider.g.cs

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_BProxy : Serde.ISerializeProvider<Some.Nested.Namespace.Base.B>
    {
        static global::Serde.ISerialize<Some.Nested.Namespace.Base.B> global::Serde.ISerializeProvider<Some.Nested.Namespace.Base.B>.Instance { get; }
            = new Some.Nested.Namespace.Base._m_BProxy._SerObj();
    }
}
