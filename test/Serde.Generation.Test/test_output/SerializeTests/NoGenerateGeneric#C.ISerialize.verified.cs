//HintName: C.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class C<T> : Serde.ISerializeProvider<C<T>>
{
    static ISerialize<C<T>> ISerializeProvider<C<T>>.SerializeInstance
        => CSerializeProxy.Instance;

    sealed partial class CSerializeProxy :Serde.ISerialize<C<T>>
    {
        void global::Serde.ISerialize<C<T>>.Serialize(C<T> value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<C<T>>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.End(_l_info);
        }
        public static readonly CSerializeProxy Instance = new();
        private CSerializeProxy() { }

    }
}
