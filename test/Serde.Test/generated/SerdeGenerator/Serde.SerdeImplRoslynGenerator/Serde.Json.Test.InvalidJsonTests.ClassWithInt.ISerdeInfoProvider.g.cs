
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithInt
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ClassWithInt",
            typeof(Serde.Json.Test.InvalidJsonTests.ClassWithInt).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
