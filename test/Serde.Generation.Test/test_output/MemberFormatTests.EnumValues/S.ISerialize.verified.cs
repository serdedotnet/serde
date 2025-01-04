//HintName: S.ISerialize.cs

#nullable enable

using System;
using Serde;
partial struct S : Serde.ISerializeProvider<S>
{
    static ISerialize<S> ISerializeProvider<S>.SerializeInstance
        => SSerializeProxy.Instance;

    sealed partial class SSerializeProxy :Serde.ISerialize<S>
    {
        void global::Serde.ISerialize<S>.Serialize(S value, global::Serde.ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<S>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<ColorEnum,ColorEnumProxy>(_l_serdeInfo,0,value.E);
            type.End();
        }
        public static readonly SSerializeProxy Instance = new();
        private SSerializeProxy() { }

    }
}
