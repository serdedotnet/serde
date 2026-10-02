
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial class SmallEnumProxy : global::Serde.ISerdeInfoProvider
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
            "SmallEnum",
            typeof(Serde.Test.JsonDeserializeTests.SmallEnum).GetCustomAttributesData(),
            global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(),
            new (string, System.Reflection.MemberInfo?)[] {
                ("a", typeof(Serde.Test.JsonDeserializeTests.SmallEnum).GetField("A")),
                ("b", typeof(Serde.Test.JsonDeserializeTests.SmallEnum).GetField("B"))
            }
        );
    }
}
