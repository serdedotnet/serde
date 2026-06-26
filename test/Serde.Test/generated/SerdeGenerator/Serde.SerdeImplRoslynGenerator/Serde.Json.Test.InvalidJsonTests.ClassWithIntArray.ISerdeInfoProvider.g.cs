
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithIntArray
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ClassWithIntArray",
            typeof(Serde.Json.Test.InvalidJsonTests.ClassWithIntArray).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int[], Serde.ArrayProxy.De<int, global::Serde.I32Proxy>>())
            }
        );
    }
}
