//HintName: S2.ISerialize.cs

#nullable enable

using System;
using Serde;
partial struct S2 : Serde.ISerializeProvider<S2>
{
    static ISerialize<S2> ISerializeProvider<S2>.SerializeInstance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<S2>
    {
        void global::Serde.ISerialize<S2>.Serialize(S2 value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<S2>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteBoxedValue<ColorEnum, ColorEnumProxy>(_l_info, 0, value.E);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
