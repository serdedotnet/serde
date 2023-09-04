
namespace Serde.Test
{
    partial record AllInOne
    {
        internal readonly partial record struct ColorEnumWrap(ColorEnum Value);
    }
}