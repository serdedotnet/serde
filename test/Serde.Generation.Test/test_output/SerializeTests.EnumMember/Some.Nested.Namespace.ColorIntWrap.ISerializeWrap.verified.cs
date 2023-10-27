//HintName: Some.Nested.Namespace.ColorIntWrap.ISerializeWrap.cs

namespace Some.Nested.Namespace
{
    partial record struct ColorIntWrap : Serde.ISerializeWrap<ColorInt, ColorIntWrap>
    {
        static ColorIntWrap Serde.ISerializeWrap<ColorInt, ColorIntWrap>.Create(ColorInt value) => new(value);
    }
}