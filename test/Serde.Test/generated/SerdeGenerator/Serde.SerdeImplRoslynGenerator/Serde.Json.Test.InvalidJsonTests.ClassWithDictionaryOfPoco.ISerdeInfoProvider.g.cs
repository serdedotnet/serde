
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithDictionaryOfPoco
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ClassWithDictionaryOfPoco",
        typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Generic.Dictionary<string, Serde.Json.Test.Poco>, Serde.DictProxy.De<string, Serde.Json.Test.Poco, global::Serde.StringProxy, Serde.Json.Test.Poco>>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco).GetProperty("Obj"))
        }
        );
    }
}
