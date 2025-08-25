
#nullable enable

namespace Serde.Test;

partial class DuplicateKeyTests
{
    partial struct AllowDuplicatesType
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "AllowDuplicatesType",
        typeof(Serde.Test.DuplicateKeyTests.AllowDuplicatesType).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("name", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.DuplicateKeyTests.AllowDuplicatesType).GetProperty("Name")),
            ("value", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.DuplicateKeyTests.AllowDuplicatesType).GetProperty("Value"))
        }
        );
    }
}
