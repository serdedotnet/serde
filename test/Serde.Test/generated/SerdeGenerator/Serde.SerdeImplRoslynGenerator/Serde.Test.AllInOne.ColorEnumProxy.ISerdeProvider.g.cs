
namespace Serde.Test;

partial record AllInOne
{
    partial class ColorEnumProxy : Serde.ISerdeProvider<Serde.Test.AllInOne.ColorEnumProxy, Serde.Test.AllInOne.ColorEnumProxy, Serde.Test.AllInOne.ColorEnum>
    {
        static Serde.Test.AllInOne.ColorEnumProxy global::Serde.ISerdeProvider<Serde.Test.AllInOne.ColorEnumProxy, Serde.Test.AllInOne.ColorEnumProxy, Serde.Test.AllInOne.ColorEnum>.Instance { get; }
            = new Serde.Test.AllInOne.ColorEnumProxy();
    }
}
