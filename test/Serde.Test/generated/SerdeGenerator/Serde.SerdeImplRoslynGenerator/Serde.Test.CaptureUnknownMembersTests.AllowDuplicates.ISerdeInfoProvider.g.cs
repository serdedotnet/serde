
#nullable enable

namespace Serde.Test;

partial class CaptureUnknownMembersTests
{
    partial class AllowDuplicates
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "AllowDuplicates",
            typeof(Serde.Test.CaptureUnknownMembersTests.AllowDuplicates).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
            }
        );
    }
}
