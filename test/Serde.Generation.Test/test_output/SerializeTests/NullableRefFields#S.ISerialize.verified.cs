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
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<S<T1, T2, T3, T4, T5>>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeFieldIfNotNull<string?,Serde.NullableRefProxy.Serialize<string,global::Serde.StringProxy>>(_l_serdeInfo,0,value.FS);
            type.SerializeField<T1,T1>(_l_serdeInfo,1,value.F1);
            type.SerializeFieldIfNotNull<T2,T2>(_l_serdeInfo,2,value.F2);
            type.SerializeFieldIfNotNull<T3?,Serde.NullableRefProxy.Serialize<T3,T3>>(_l_serdeInfo,3,value.F3);
            type.SerializeFieldIfNotNull<T4,T4>(_l_serdeInfo,4,value.F4);
            type.End();
        }
        public static readonly SSerializeProxy Instance = new();
        private SSerializeProxy() { }

    }
}
