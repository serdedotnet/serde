
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial record NoComma
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "NoComma",
        typeof(Serde.Json.Test.InvalidJsonTests.NoComma).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("a", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Json.Test.InvalidJsonTests.NoComma).GetProperty("A")),
            ("b", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Json.Test.InvalidJsonTests.NoComma).GetProperty("B"))
        }
        );
    }
}
