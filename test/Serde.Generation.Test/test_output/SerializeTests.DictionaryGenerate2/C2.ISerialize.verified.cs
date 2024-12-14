﻿//HintName: C2.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class C2 : Serde.ISerializeProvider<C2>
{
    static ISerialize<C2> ISerializeProvider<C2>.SerializeInstance => C2SerializeProxy.Instance;

    sealed class C2SerializeProxy : Serde.ISerialize<C2>
    {
        void ISerialize<C2>.Serialize(C2 value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<C2>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<System.Collections.Generic.Dictionary<string, C>, Serde.DictProxy.Serialize<string, C, global::Serde.StringProxy, C>>(_l_serdeInfo, 0, value.Map);
            type.End();
        }

        public static readonly C2SerializeProxy Instance = new();
        private C2SerializeProxy()
        {
        }
    }
}