
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithDictionaryOfPocoList
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ClassWithDictionaryOfPocoList",
        typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPocoList).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<Serde.Json.Test.Poco>>, Serde.DictProxy.De<string, System.Collections.Generic.List<Serde.Json.Test.Poco>, global::Serde.StringProxy, Serde.ListProxy.De<Serde.Json.Test.Poco, Serde.Json.Test.Poco>>>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPocoList).GetProperty("Obj"))
        }
        );
    }
}
