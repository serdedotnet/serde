
namespace Serde.Test
{
    partial record AllInOne
    {
        partial record struct ColorEnumWrap : Serde.ISerializeWrap<ColorEnum, ColorEnumWrap>
        {
            static ColorEnumWrap Serde.ISerializeWrap<ColorEnum, ColorEnumWrap>.Create(ColorEnum value) => new(value);
        }
    }
}