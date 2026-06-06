
#nullable enable

namespace Serde.Test;

partial class RoundtripTests
{
    partial struct ForeignPointProxy
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ForeignPoint",
        typeof(Serde.Test.RoundtripTests.ForeignPointProxy).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("X", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.RoundtripTests.ForeignPointProxy).GetField("X")),
            ("Y", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.RoundtripTests.ForeignPointProxy).GetField("Y"))
        }
        );
    }
}
