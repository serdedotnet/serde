//HintName: Some.Nested.Namespace.C.ISerialize.cs

#nullable enable
using Serde;

namespace Some.Nested.Namespace
{
    partial class C : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType("C", 4);
            type.SerializeField("colorInt", new ColorIntWrap(this.ColorInt));
            type.SerializeField("colorByte", new ColorByteWrap(this.ColorByte));
            type.SerializeField("colorLong", new ColorLongWrap(this.ColorLong));
            type.SerializeField("colorULong", new ColorULongWrap(this.ColorULong));
            type.End();
        }
    }
}