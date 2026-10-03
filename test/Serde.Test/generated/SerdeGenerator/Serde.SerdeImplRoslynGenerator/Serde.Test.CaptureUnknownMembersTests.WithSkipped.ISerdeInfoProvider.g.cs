
#nullable enable

namespace Serde.Test;

partial class CaptureUnknownMembersTests
{
    partial class WithSkipped
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "WithSkipped",
            typeof(Serde.Test.CaptureUnknownMembersTests.WithSkipped).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>()),
                new("computed", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
                {
                    MemberInfo = typeof(Serde.Test.CaptureUnknownMembersTests.WithSkipped).GetProperty("Computed"),
                    IsOptional = true,
                }
            }
        );
    }
}
