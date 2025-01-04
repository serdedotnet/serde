//HintName: C.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class C : Serde.ISerializeProvider<C>
{
    static ISerialize<C> ISerializeProvider<C>.SerializeInstance
        => CSerializeProxy.Instance;

    sealed partial class CSerializeProxy :Serde.ISerialize<C>
    {
        void global::Serde.ISerialize<C>.Serialize(C value, global::Serde.ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<C>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<System.Collections.Generic.Dictionary<string, int>,Serde.DictProxy.Serialize<string,int,global::Serde.StringProxy,global::Serde.Int32Proxy>>(_l_serdeInfo,0,value.Map);
            type.End();
        }
        public static readonly CSerializeProxy Instance = new();
        private CSerializeProxy() { }

    }
}
