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
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<S>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteBoxedField<ColorEnum, ColorEnumProxy>(_l_info, 0, value.E);
            _l_type.End(_l_info);
        }
        public static readonly SSerializeProxy Instance = new();
        private SSerializeProxy() { }

    }
}
