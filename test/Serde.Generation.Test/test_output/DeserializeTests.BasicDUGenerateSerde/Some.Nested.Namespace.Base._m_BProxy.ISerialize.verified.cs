﻿//HintName: Some.Nested.Namespace.Base._m_BProxy.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_BProxy : Serde.ISerializeProvider<Some.Nested.Namespace.Base.B>
    {
        static ISerialize<Some.Nested.Namespace.Base.B> ISerializeProvider<Some.Nested.Namespace.Base.B>.Instance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<Some.Nested.Namespace.Base.B>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Some.Nested.Namespace.Base._m_BProxy.s_serdeInfo;

            void global::Serde.ISerialize<Some.Nested.Namespace.Base.B>.Serialize(Some.Nested.Namespace.Base.B value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteString(_l_info, 0, value.Y);
                _l_type.End(_l_info);
            }
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
