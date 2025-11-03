
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record TimeOnlyWrap
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "TimeOnlyWrap",
        typeof(Serde.Test.JsonSerializerTests.TimeOnlyWrap).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("value", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.TimeOnly, Serde.TimeOnlyProxy>(), typeof(Serde.Test.JsonSerializerTests.TimeOnlyWrap).GetProperty("Value"))
        }
        );
    }
}
