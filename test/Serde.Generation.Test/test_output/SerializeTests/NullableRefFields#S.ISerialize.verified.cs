//HintName: S.ISerialize.cs

#nullable enable

using System;
using Serde;
partial struct S<T1, T2, T3, T4, T5> : Serde.ISerializeProvider<S<T1, T2, T3, T4, T5>>
{
    static ISerialize<S<T1, T2, T3, T4, T5>> ISerializeProvider<S<T1, T2, T3, T4, T5>>.SerializeInstance
        => SSerializeProxy.Instance;

    sealed partial class SSerializeProxy :Serde.ISerialize<S<T1, T2, T3, T4, T5>>
    {
        void global::Serde.ISerialize<S<T1, T2, T3, T4, T5>>.Serialize(S<T1, T2, T3, T4, T5> value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<S<T1, T2, T3, T4, T5>>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteStringIfNotNull(_l_info, 0, value.FS);
            _l_type.WriteBoxedField<T1, T1>(_l_info, 1, value.F1);
            _l_type.WriteBoxedFieldIfNotNull<T2, T2>(_l_info, 2, value.F2);
            _l_type.WriteFieldIfNotNull<T3?, Serde.NullableRefProxy.Serialize<T3, T3>>(_l_info, 3, value.F3);
            _l_type.WriteFieldIfNotNull<T4, T4>(_l_info, 4, value.F4);
            _l_type.End(_l_info);
        }
        public static readonly SSerializeProxy Instance = new();
        private SSerializeProxy() { }

    }
}
