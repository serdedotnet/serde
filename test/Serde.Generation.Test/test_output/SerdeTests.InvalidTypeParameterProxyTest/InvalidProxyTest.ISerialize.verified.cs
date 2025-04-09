//HintName: InvalidProxyTest.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class InvalidProxyTest<T> : Serde.ISerializeProvider<InvalidProxyTest<T>>
{
    static ISerialize<InvalidProxyTest<T>> ISerializeProvider<InvalidProxyTest<T>>.Instance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<InvalidProxyTest<T>>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => InvalidProxyTest<T>.s_serdeInfo;

        void global::Serde.ISerialize<InvalidProxyTest<T>>.Serialize(InvalidProxyTest<T> value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
