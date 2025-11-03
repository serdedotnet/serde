
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record DateOnlyWrap
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "DateOnlyWrap",
        typeof(Serde.Test.JsonSerializerTests.DateOnlyWrap).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("value", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.DateOnly, Serde.DateOnlyProxy>(), typeof(Serde.Test.JsonSerializerTests.DateOnlyWrap).GetProperty("Value"))
        }
        );
    }
}
