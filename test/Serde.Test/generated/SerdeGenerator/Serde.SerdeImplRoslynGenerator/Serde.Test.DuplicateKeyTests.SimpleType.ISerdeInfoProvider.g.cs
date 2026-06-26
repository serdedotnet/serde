
#nullable enable

namespace Serde.Test;

partial class DuplicateKeyTests
{
    partial struct SimpleType
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "SimpleType",
            typeof(Serde.Test.DuplicateKeyTests.SimpleType).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("name", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("value", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
