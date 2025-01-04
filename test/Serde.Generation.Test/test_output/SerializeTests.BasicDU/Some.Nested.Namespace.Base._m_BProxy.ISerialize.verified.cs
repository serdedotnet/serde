//HintName: Some.Nested.Namespace.Base._m_BProxy.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_BProxy : Serde.ISerializeProvider<Some.Nested.Namespace.Base.B>
    {
        static ISerialize<Some.Nested.Namespace.Base.B> ISerializeProvider<Some.Nested.Namespace.Base.B>.SerializeInstance
            => _m_BProxySerializeProxy.Instance;

        sealed partial class _m_BProxySerializeProxy :Serde.ISerialize<Some.Nested.Namespace.Base.B>
        {
            void global::Serde.ISerialize<Some.Nested.Namespace.Base.B>.Serialize(Some.Nested.Namespace.Base.B value, global::Serde.ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<_m_BProxy>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<string,global::Serde.StringProxy>(_l_serdeInfo,0,value.Y);
                type.End();
            }
            public static readonly _m_BProxySerializeProxy Instance = new();
            private _m_BProxySerializeProxy() { }

        }
    }
}
