//HintName: Some.Nested.Namespace.ColorULongWrap.ISerializeWrap.cs

namespace Some.Nested.Namespace
{
    partial record struct ColorULongWrap : Serde.ISerializeWrap<ColorULong, ColorULongWrap>
    {
        static ColorULongWrap Serde.ISerializeWrap<ColorULong, ColorULongWrap>.Create(ColorULong value) => new(value);
    }
}