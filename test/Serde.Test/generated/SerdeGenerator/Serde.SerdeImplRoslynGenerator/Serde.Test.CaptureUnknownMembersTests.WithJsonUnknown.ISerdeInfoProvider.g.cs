
#nullable enable

namespace Serde.Test;

partial class CaptureUnknownMembersTests
{
    partial class WithJsonUnknown
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "WithJsonUnknown",
            typeof(Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>()),
                new("age", global::Serde.SerdeInfoProvider.GetSerializeInfo<int?, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>())
                {
                    IsOptional = true,
                }
            }
        );
    }
}
