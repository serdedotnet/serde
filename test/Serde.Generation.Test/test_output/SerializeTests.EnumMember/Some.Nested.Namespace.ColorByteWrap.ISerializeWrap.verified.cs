//HintName: Some.Nested.Namespace.ColorByteWrap.ISerializeWrap.cs

namespace Some.Nested.Namespace
{
    partial record struct ColorByteWrap : Serde.ISerializeWrap<ColorByte, ColorByteWrap>
    {
        static ColorByteWrap Serde.ISerializeWrap<ColorByte, ColorByteWrap>.Create(ColorByte value) => new(value);
    }
}