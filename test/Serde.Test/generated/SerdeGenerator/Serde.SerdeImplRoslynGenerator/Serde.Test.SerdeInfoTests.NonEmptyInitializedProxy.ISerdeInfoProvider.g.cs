
#nullable enable

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial class NonEmptyInitializedProxy
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "InitializedTarget",
            typeof(Serde.Test.SerdeInfoTests.NonEmptyInitializedProxy).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("number", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
                {
                    IsOptional = true,
                },
                new("text", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>())
                {
                    IsOptional = true,
                }
            }
        );
    }
}
