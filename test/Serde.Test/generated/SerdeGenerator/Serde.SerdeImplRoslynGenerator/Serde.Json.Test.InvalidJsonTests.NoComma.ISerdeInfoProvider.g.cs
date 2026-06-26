
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial record NoComma
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "NoComma",
            typeof(Serde.Json.Test.InvalidJsonTests.NoComma).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("a", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>()),
                new("b", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
