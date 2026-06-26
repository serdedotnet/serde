
#nullable enable

namespace Serde.Test;

partial class RoundtripTests
{
    partial struct ForeignPointProxy
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ForeignPoint",
            typeof(Serde.Test.RoundtripTests.ForeignPointProxy).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("X", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
                new("Y", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
