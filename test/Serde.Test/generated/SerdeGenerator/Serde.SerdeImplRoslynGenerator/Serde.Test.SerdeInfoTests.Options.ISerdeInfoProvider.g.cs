
#nullable enable

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record Options
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Options",
            typeof(Serde.Test.SerdeInfoTests.Options).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("first", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>())
                {
                    MemberInfo = typeof(Serde.Test.SerdeInfoTests.Options).GetProperty("First"),
                }
            }
        );
    }
}
