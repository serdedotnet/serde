
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithDictionaryOfIntArray : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "ClassWithDictionaryOfIntArray",
            typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfIntArray).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
                ("obj", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictProxy.De<string, int[], global::Serde.StringProxy, Serde.ArrayProxy.De<int, global::Serde.I32Proxy>>>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfIntArray).GetProperty("Obj"))
            }
        );
    }
}