//HintName: Some.Nested.Namespace.C.ISerialize.g.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial class C
{
    sealed partial class _SerObj : Serde.ISerialize<Some.Nested.Namespace.C>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Some.Nested.Namespace.C.s_serdeInfo;

        void global::Serde.ISerialize<Some.Nested.Namespace.C>.Serialize(Some.Nested.Namespace.C value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteBoxedValue<Some.Nested.Namespace.ColorInt, Some.Nested.Namespace.ColorIntProxy>(_l_info, 0, value.ColorInt);
            _l_type.WriteBoxedValue<Some.Nested.Namespace.ColorByte, Some.Nested.Namespace.ColorByteProxy>(_l_info, 1, value.ColorByte);
            _l_type.WriteBoxedValue<Some.Nested.Namespace.ColorLong, Some.Nested.Namespace.ColorLongProxy>(_l_info, 2, value.ColorLong);
            _l_type.WriteBoxedValue<Some.Nested.Namespace.ColorULong, Some.Nested.Namespace.ColorULongProxy>(_l_info, 3, value.ColorULong);
            _l_type.End(_l_info);
        }

    }
}
