﻿//HintName: S.ISerialize.cs

#nullable enable

using System;
using Serde;
partial struct S<T1, T2, TSerialize> : Serde.ISerializeProvider<S<T1, T2, TSerialize>>
{
    static ISerialize<S<T1, T2, TSerialize>> ISerializeProvider<S<T1, T2, TSerialize>>.SerializeInstance
        => SSerializeProxy.Instance;

    sealed partial class SSerializeProxy :Serde.ISerialize<S<T1, T2, TSerialize>>
    {
        void global::Serde.ISerialize<S<T1, T2, TSerialize>>.Serialize(S<T1, T2, TSerialize> value, global::Serde.ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<S<T1, T2, TSerialize>>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeFieldIfNotNull<int?,Serde.NullableProxy.Serialize<int,global::Serde.Int32Proxy>>(_l_serdeInfo,0,value.FI);
            type.SerializeFieldIfNotNull<TSerialize?,Serde.NullableProxy.Serialize<TSerialize,TSerialize>>(_l_serdeInfo,3,value.F3);
            type.End();
        }
        public static readonly SSerializeProxy Instance = new();
        private SSerializeProxy() { }

    }
}
