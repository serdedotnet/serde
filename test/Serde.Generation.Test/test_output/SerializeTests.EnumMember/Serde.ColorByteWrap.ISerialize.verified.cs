//HintName: Serde.ColorByteWrap.ISerialize.cs

#nullable enable
using Serde;

namespace Serde
{
    partial record struct ColorByteWrap : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var name = Value switch
            {
                Some.Nested.Namespace.ColorByte.Red => "red",
                Some.Nested.Namespace.ColorByte.Green => "green",
                Some.Nested.Namespace.ColorByte.Blue => "blue",
                _ => null
            };
            serializer.SerializeEnumValue("ColorByte", name, new ByteWrap((byte)Value));
        }
    }
}