
#nullable enable

namespace Serde.Test;

partial class CaptureUnknownMembersTests
{
    partial class WithIntUnknown
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "WithIntUnknown",
            typeof(Serde.Test.CaptureUnknownMembersTests.WithIntUnknown).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
            }
        );
    }
}
