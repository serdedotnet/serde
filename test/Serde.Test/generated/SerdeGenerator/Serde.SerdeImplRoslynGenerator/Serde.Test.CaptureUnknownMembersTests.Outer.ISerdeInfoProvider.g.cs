
#nullable enable

namespace Serde.Test;

partial class CaptureUnknownMembersTests
{
    partial record Outer
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Outer",
            typeof(Serde.Test.CaptureUnknownMembersTests.Outer).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("id", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>()),
                new("inner", global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown, Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown>())
            }
        );
    }
}
