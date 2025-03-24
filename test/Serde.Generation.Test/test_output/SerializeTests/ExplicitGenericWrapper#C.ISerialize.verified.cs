//HintName: C.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class C : Serde.ISerializeProvider<C>
{
    static ISerialize<C> ISerializeProvider<C>.Instance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<C>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => C.s_serdeInfo;

        void global::Serde.ISerialize<C>.Serialize(C value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteBoxedValue<S<int>, SWrap.Ser<int, global::Serde.I32Proxy>>(_l_info, 0, value.S);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
