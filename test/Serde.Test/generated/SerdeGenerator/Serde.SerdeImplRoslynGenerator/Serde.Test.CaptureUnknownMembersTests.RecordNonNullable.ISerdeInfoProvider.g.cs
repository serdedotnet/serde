
#nullable enable

namespace Serde.Test;

partial class CaptureUnknownMembersTests
{
    partial record RecordNonNullable
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "RecordNonNullable",
            typeof(Serde.Test.CaptureUnknownMembersTests.RecordNonNullable).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
            }
        );
    }
}
