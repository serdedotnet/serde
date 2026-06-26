
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithDictionaryOfPoco
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ClassWithDictionaryOfPoco",
            typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Generic.Dictionary<string, Serde.Json.Test.Poco>, Serde.DictProxy.De<string, Serde.Json.Test.Poco, global::Serde.StringProxy, Serde.Json.Test.Poco>>())
            }
        );
    }
}
