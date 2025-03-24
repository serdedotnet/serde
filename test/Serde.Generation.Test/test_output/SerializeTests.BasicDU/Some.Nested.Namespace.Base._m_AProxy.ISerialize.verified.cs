﻿//HintName: Some.Nested.Namespace.Base._m_AProxy.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_AProxy : Serde.ISerializeProvider<Some.Nested.Namespace.Base.A>
    {
        static ISerialize<Some.Nested.Namespace.Base.A> ISerializeProvider<Some.Nested.Namespace.Base.A>.Instance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<Some.Nested.Namespace.Base.A>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Some.Nested.Namespace.Base._m_AProxy.s_serdeInfo;

            void global::Serde.ISerialize<Some.Nested.Namespace.Base.A>.Serialize(Some.Nested.Namespace.Base.A value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteI32(_l_info, 0, value.X);
                _l_type.End(_l_info);
            }
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
