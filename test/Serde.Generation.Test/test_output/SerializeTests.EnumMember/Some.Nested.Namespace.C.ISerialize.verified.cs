//HintName: Some.Nested.Namespace.C.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    partial class C : Serde.ISerialize<Some.Nested.Namespace.C>
    {
        void ISerialize<Some.Nested.Namespace.C>.Serialize(Some.Nested.Namespace.C value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<C>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<Some.Nested.Namespace.ColorInt, Some.Nested.Namespace.ColorIntWrap>(_l_serdeInfo, 0, value.ColorInt);
            type.SerializeField<Some.Nested.Namespace.ColorByte, Some.Nested.Namespace.ColorByteWrap>(_l_serdeInfo, 1, value.ColorByte);
            type.SerializeField<Some.Nested.Namespace.ColorLong, Some.Nested.Namespace.ColorLongWrap>(_l_serdeInfo, 2, value.ColorLong);
            type.SerializeField<Some.Nested.Namespace.ColorULong, Some.Nested.Namespace.ColorULongWrap>(_l_serdeInfo, 3, value.ColorULong);
            type.End();
        }
    }
}