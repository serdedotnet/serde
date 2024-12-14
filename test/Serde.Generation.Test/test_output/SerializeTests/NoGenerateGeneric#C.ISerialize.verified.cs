//HintName: C.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class C<T> : Serde.ISerializeProvider<C<T>>
{
    static ISerialize<C<T>> ISerializeProvider<C<T>>.SerializeInstance => CSerializeProxy.Instance;

    sealed class CSerializeProxy : Serde.ISerialize<C<T>>
    {
        void ISerialize<C<T>>.Serialize(C<T> value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<C<T>>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.End();
        }

        public static readonly CSerializeProxy Instance = new();
        private CSerializeProxy()
        {
        }
    }
}