//HintName: Some.Nested.Namespace.Base.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    partial record Base : Serde.ISerializeProvider<Some.Nested.Namespace.Base>
    {
        static ISerialize<Some.Nested.Namespace.Base> ISerializeProvider<Some.Nested.Namespace.Base>.SerializeInstance => BaseSerializeProxy.Instance;

        sealed class BaseSerializeProxy : Serde.ISerialize<Some.Nested.Namespace.Base>
        {
            void ISerialize<Some.Nested.Namespace.Base>.Serialize(Some.Nested.Namespace.Base value, ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Some.Nested.Namespace.Base>();
                var _l_type = serializer.SerializeType(_l_serdeInfo);
                switch (value)
                {
                    case Some.Nested.Namespace.Base.A c:
                        _l_type.SerializeField<Some.Nested.Namespace.Base.A, _m_AProxy>(_l_serdeInfo, 0, c);
                        break;
                    case Some.Nested.Namespace.Base.B c:
                        _l_type.SerializeField<Some.Nested.Namespace.Base.B, _m_BProxy>(_l_serdeInfo, 1, c);
                        break;
                }

                _l_type.End();
            }

            public static readonly BaseSerializeProxy Instance = new();
            private BaseSerializeProxy()
            {
            }
        }
    }
}