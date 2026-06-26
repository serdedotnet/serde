
#nullable enable

namespace Serde.Test;

partial class DuplicateKeyTests
{
    partial struct SimpleTypeAllowDuplicates
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "SimpleTypeAllowDuplicates",
            typeof(Serde.Test.DuplicateKeyTests.SimpleTypeAllowDuplicates).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("name", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                new("value", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
