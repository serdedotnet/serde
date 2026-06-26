
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class SkipClass
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "SkipClass",
            typeof(Serde.Json.Test.InvalidJsonTests.SkipClass).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("c", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
