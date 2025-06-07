//HintName: C.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial class C
{
    sealed partial class _SerObj : Serde.ISerialize<C>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => C.s_serdeInfo;

        void global::Serde.ISerialize<C>.Serialize(C value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteBoxedValueIfNotNull<Rgb?, Serde.NullableProxy.Ser<Rgb, RgbProxy>>(_l_info, 0, value.ColorOpt);
            _l_type.End(_l_info);
        }

    }
}
