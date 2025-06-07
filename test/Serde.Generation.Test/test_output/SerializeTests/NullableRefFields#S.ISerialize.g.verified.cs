//HintName: S.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial struct S<T1, T2, T3, T4, T5>
{
    sealed partial class _SerObj : Serde.ISerialize<S<T1, T2, T3, T4, T5>>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => S<T1, T2, T3, T4, T5>.s_serdeInfo;

        void global::Serde.ISerialize<S<T1, T2, T3, T4, T5>>.Serialize(S<T1, T2, T3, T4, T5> value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteStringIfNotNull(_l_info, 0, value.FS);
            _l_type.WriteBoxedValue<T1, T1>(_l_info, 1, value.F1);
            _l_type.WriteBoxedValueIfNotNull<T2, T2>(_l_info, 2, value.F2);
            _l_type.WriteValueIfNotNull<T3?, Serde.NullableRefProxy.Ser<T3, T3>>(_l_info, 3, value.F3);
            _l_type.WriteValueIfNotNull<T4, T4>(_l_info, 4, value.F4);
            _l_type.End(_l_info);
        }

    }
}
