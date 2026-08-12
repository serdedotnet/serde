
#nullable enable

namespace Serde.Test;

partial class RoundtripTests
{
    partial record ProxiedContainer
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ProxiedContainer",
            typeof(Serde.Test.RoundtripTests.ProxiedContainer).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("Point", global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.RoundtripTests.ForeignPoint, Serde.Test.RoundtripTests.ForeignPointProxy>()),
                new("OptionalPoint", global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.RoundtripTests.ForeignPoint?, Serde.NullableRefProxy.Ser<Serde.Test.RoundtripTests.ForeignPoint, Serde.Test.RoundtripTests.ForeignPointProxy>>())
            }
        );
    }
}
