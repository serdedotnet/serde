
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class PocoWithParameterizedCtor
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "PocoWithParameterizedCtor",
        typeof(Serde.Json.Test.InvalidJsonTests.PocoWithParameterizedCtor).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Json.Test.InvalidJsonTests.PocoWithParameterizedCtor).GetProperty("Obj"))
        }
        );
    }
}
