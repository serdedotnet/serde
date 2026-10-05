
#nullable enable

namespace Serde.Test;

partial class CaptureUnknownMembersTests
{
    partial record RecordUnknown
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "RecordUnknown",
            typeof(Serde.Test.CaptureUnknownMembersTests.RecordUnknown).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
            }
        );
    }
}
