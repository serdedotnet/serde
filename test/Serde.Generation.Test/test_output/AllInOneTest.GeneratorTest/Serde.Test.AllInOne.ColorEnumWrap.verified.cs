//HintName: Serde.Test.AllInOne.ColorEnumWrap.cs

namespace Serde.Test
{
    partial record AllInOne
    {
        readonly partial record struct ColorEnumWrap(ColorEnum Value);
    }
}