//HintName: C.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.ISerializeProvider<C>
{
    static ISerialize<C> ISerializeProvider<C>.SerializeInstance => CSerializeProxy.Instance;

    sealed class CSerializeProxy : Serde.ISerialize<C>
    {
        void ISerialize<C>.Serialize(C value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<C>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<S<int>, SWrap.Serialize<int, global::Serde.Int32Proxy>>(_l_serdeInfo, 0, value.S);
            type.End();
        }

        public static readonly CSerializeProxy Instance = new();
        private CSerializeProxy()
        {
        }
    }
}