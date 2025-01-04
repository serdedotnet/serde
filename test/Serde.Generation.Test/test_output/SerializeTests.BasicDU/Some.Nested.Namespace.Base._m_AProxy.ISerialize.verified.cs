//HintName: Some.Nested.Namespace.Base._m_AProxy.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_AProxy : Serde.ISerializeProvider<Some.Nested.Namespace.Base.A>
    {
        static ISerialize<Some.Nested.Namespace.Base.A> ISerializeProvider<Some.Nested.Namespace.Base.A>.SerializeInstance
            => _m_AProxySerializeProxy.Instance;

        sealed partial class _m_AProxySerializeProxy :Serde.ISerialize<Some.Nested.Namespace.Base.A>
        {
            void global::Serde.ISerialize<Some.Nested.Namespace.Base.A>.Serialize(Some.Nested.Namespace.Base.A value, global::Serde.ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<_m_AProxy>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<int,global::Serde.Int32Proxy>(_l_serdeInfo,0,value.X);
                type.End();
            }
            public static readonly _m_AProxySerializeProxy Instance = new();
            private _m_AProxySerializeProxy() { }

        }
    }
}
