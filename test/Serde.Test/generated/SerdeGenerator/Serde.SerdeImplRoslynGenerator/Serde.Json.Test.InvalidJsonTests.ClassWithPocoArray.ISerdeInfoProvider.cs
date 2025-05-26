
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithPocoArray
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ClassWithPocoArray",
        typeof(Serde.Json.Test.InvalidJsonTests.ClassWithPocoArray).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<Serde.Json.Test.Poco[], Serde.ArrayProxy.De<Serde.Json.Test.Poco, Serde.Json.Test.Poco>>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithPocoArray).GetProperty("Obj"))
        }
        );
    }
}
