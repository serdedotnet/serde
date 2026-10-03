
#nullable enable

namespace Serde.Test;

partial class CaptureUnknownMembersTests
{
    partial class WithStringUnknown
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "WithStringUnknown",
            typeof(Serde.Test.CaptureUnknownMembersTests.WithStringUnknown).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
            }
        );
    }
}
