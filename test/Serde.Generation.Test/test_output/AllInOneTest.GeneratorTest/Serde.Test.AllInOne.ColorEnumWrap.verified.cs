//HintName: Serde.Test.AllInOne.ColorEnumWrap.cs

namespace Serde.Test
{
    partial record AllInOne
    {
        internal readonly partial record struct ColorEnumWrap(ColorEnum Value);
    }
}