﻿//HintName: S1.ISerialize.cs

#nullable enable

using System;
using Serde;
partial struct S1 : Serde.ISerializeProvider<S1>
{
    static ISerialize<S1> ISerializeProvider<S1>.SerializeInstance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<S1>
    {
        void global::Serde.ISerialize<S1>.Serialize(S1 value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<S1>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
