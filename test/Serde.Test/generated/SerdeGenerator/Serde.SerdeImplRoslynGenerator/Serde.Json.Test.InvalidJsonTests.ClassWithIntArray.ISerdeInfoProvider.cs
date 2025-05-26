
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithIntArray
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ClassWithIntArray",
        typeof(Serde.Json.Test.InvalidJsonTests.ClassWithIntArray).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int[], Serde.ArrayProxy.De<int, global::Serde.I32Proxy>>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithIntArray).GetProperty("Obj"))
        }
        );
    }
}
