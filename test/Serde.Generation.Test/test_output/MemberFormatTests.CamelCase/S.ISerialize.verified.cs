//HintName: S.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.ISerializeProvider<S>
{
    static ISerialize<S> ISerializeProvider<S>.SerializeInstance => SSerializeProxy.Instance;

    sealed class SSerializeProxy : Serde.ISerialize<S>
    {
        void ISerialize<S>.Serialize(S value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<S>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<int, global::Serde.Int32Proxy>(_l_serdeInfo, 0, value.One);
            type.SerializeField<int, global::Serde.Int32Proxy>(_l_serdeInfo, 1, value.TwoWord);
            type.End();
        }

        public static readonly SSerializeProxy Instance = new();
        private SSerializeProxy()
        {
        }
    }
}