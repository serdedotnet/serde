
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithPocoArray
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ClassWithPocoArray",
            typeof(Serde.Json.Test.InvalidJsonTests.ClassWithPocoArray).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<Serde.Json.Test.Poco[], Serde.ArrayProxy.De<Serde.Json.Test.Poco, Serde.Json.Test.Poco>>())
            }
        );
    }
}
