//HintName: Some.Nested.Namespace.C.ISerialize`1.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    partial class C : Serde.ISerialize<Some.Nested.Namespace.C>
    {
        void ISerialize<Some.Nested.Namespace.C>.Serialize(Some.Nested.Namespace.C value, ISerializer serializer)
        {
            var type = serializer.SerializeType("C", 4);
            type.SerializeField<Some.Nested.Namespace.ColorInt, Some.Nested.Namespace.ColorIntWrap>("colorInt", value.ColorInt);
            type.SerializeField<Some.Nested.Namespace.ColorByte, Some.Nested.Namespace.ColorByteWrap>("colorByte", value.ColorByte);
            type.SerializeField<Some.Nested.Namespace.ColorLong, Some.Nested.Namespace.ColorLongWrap>("colorLong", value.ColorLong);
            type.SerializeField<Some.Nested.Namespace.ColorULong, Some.Nested.Namespace.ColorULongWrap>("colorULong", value.ColorULong);
            type.End();
        }
    }
}