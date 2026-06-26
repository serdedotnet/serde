
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithPoco
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ClassWithPoco",
            typeof(Serde.Json.Test.InvalidJsonTests.ClassWithPoco).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<Serde.Json.Test.Poco, Serde.Json.Test.Poco>())
            }
        );
    }
}
