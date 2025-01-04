//HintName: S2.ISerialize.cs

#nullable enable

using System;
using Serde;
partial struct S2 : Serde.ISerializeProvider<S2>
{
    static ISerialize<S2> ISerializeProvider<S2>.SerializeInstance
        => S2SerializeProxy.Instance;

    sealed partial class S2SerializeProxy :Serde.ISerialize<S2>
    {
        void global::Serde.ISerialize<S2>.Serialize(S2 value, global::Serde.ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<S2>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<ColorEnum,ColorEnumProxy>(_l_serdeInfo,0,value.E);
            type.End();
        }
        public static readonly S2SerializeProxy Instance = new();
        private S2SerializeProxy() { }

    }
}
