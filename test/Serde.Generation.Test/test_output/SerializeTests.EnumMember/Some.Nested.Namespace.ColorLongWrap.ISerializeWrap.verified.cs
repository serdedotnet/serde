//HintName: Some.Nested.Namespace.ColorLongWrap.ISerializeWrap.cs

namespace Some.Nested.Namespace
{
    partial record struct ColorLongWrap : Serde.ISerializeWrap<ColorLong, ColorLongWrap>
    {
        static ColorLongWrap Serde.ISerializeWrap<ColorLong, ColorLongWrap>.Create(ColorLong value) => new(value);
    }
}