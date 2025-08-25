
#nullable enable

namespace Serde.Test;

partial class DuplicateKeyTests
{
    partial struct SimpleType
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "SimpleType",
        typeof(Serde.Test.DuplicateKeyTests.SimpleType).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("name", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.DuplicateKeyTests.SimpleType).GetProperty("Name")),
            ("value", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.DuplicateKeyTests.SimpleType).GetProperty("Value"))
        }
        );
    }
}
