//HintName: S.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial struct S<T1, T2, TSerialize>
{
    sealed partial class _SerObj : Serde.ISerialize<S<T1, T2, TSerialize>>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => S<T1, T2, TSerialize>.s_serdeInfo;

        void global::Serde.ISerialize<S<T1, T2, TSerialize>>.Serialize(S<T1, T2, TSerialize> value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_fieldCount = 2;
            if (value.FI is null) _l_fieldCount--;
            if (value.F3 is null) _l_fieldCount--;
            var _l_type = serializer.WriteType(_l_info, _l_fieldCount);
            _l_type.WriteValueIfNotNull<int, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>(_l_info, 0, value.FI);
            _l_type.WriteValueIfNotNull<TSerialize, Serde.NullableProxy.Ser<TSerialize, TSerialize>>(_l_info, 3, value.F3);
            _l_type.End(_l_info);
        }

    }
}
