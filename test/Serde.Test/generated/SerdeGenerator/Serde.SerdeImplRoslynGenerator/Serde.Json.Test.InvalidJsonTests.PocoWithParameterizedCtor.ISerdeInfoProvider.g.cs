
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class PocoWithParameterizedCtor
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "PocoWithParameterizedCtor",
            typeof(Serde.Json.Test.InvalidJsonTests.PocoWithParameterizedCtor).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
