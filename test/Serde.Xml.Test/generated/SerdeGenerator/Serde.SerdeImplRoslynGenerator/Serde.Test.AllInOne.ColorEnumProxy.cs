

namespace Serde.Test;

partial record AllInOne
{
    sealed partial class ColorEnumProxy
    {
        public static readonly ColorEnumProxy Instance = new();
        private ColorEnumProxy() { }
    }
}