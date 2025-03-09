//HintName: S.ISerialize.cs

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
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<S<T1, T2, TSerialize>>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteBoxedFieldIfNotNull<int?, Serde.NullableProxy.Serialize<int, global::Serde.I32Proxy>>(_l_info, 0, value.FI);
            _l_type.WriteBoxedFieldIfNotNull<TSerialize?, Serde.NullableProxy.Serialize<TSerialize, TSerialize>>(_l_info, 3, value.F3);
            _l_type.End(_l_info);
        }
        public static readonly SSerializeProxy Instance = new();
        private SSerializeProxy() { }

    }
}
