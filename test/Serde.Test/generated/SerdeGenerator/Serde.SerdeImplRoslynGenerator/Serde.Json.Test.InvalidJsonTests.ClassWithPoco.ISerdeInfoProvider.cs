
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithPoco
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ClassWithPoco",
        typeof(Serde.Json.Test.InvalidJsonTests.ClassWithPoco).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<Serde.Json.Test.Poco, Serde.Json.Test.Poco>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithPoco).GetProperty("Obj"))
        }
        );
    }
}
