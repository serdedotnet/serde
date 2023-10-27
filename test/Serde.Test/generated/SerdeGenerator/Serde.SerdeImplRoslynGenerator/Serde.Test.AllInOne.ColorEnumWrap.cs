
namespace Serde.Test
{
    partial record AllInOne
    {
        readonly partial record struct ColorEnumWrap(ColorEnum Value);
    }
}