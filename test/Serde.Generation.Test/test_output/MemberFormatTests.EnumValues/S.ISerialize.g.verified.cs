﻿//HintName: S.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial struct S
{
    sealed partial class _SerObj : Serde.ISerialize<S>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => S.s_serdeInfo;

        void global::Serde.ISerialize<S>.Serialize(S value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteBoxedValue<ColorEnum, ColorEnumProxy>(_l_info, 0, value.E);
            _l_type.End(_l_info);
        }

    }
}
