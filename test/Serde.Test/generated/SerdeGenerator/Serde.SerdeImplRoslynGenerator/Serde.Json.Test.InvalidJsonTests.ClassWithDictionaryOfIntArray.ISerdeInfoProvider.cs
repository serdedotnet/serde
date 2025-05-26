
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithDictionaryOfIntArray
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ClassWithDictionaryOfIntArray",
        typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfIntArray).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Generic.Dictionary<string, int[]>, Serde.DictProxy.De<string, int[], global::Serde.StringProxy, Serde.ArrayProxy.De<int, global::Serde.I32Proxy>>>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfIntArray).GetProperty("Obj"))
        }
        );
    }
}
