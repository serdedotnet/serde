//HintName: Some.Nested.Namespace.C.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    partial class C : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType("C", 4);
            type.SerializeField("colorInt"u8, new ColorIntWrap(this.ColorInt));
            type.SerializeField("colorByte"u8, new ColorByteWrap(this.ColorByte));
            type.SerializeField("colorLong"u8, new ColorLongWrap(this.ColorLong));
            type.SerializeField("colorULong"u8, new ColorULongWrap(this.ColorULong));
            type.End();
        }
    }
}