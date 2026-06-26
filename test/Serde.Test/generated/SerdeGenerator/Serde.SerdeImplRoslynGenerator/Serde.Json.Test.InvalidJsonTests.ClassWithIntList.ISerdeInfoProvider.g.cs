
#nullable enable

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithIntList
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ClassWithIntList",
            typeof(Serde.Json.Test.InvalidJsonTests.ClassWithIntList).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("obj", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Generic.List<int>, Serde.ListProxy.De<int, global::Serde.I32Proxy>>())
            }
        );
    }
}
