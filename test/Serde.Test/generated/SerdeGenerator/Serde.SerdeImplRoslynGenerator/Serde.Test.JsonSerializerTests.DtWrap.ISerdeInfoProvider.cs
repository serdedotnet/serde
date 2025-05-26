
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record DtWrap
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "DtWrap",
        typeof(Serde.Test.JsonSerializerTests.DtWrap).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("value", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.DateTime, global::Serde.DateTimeProxy>(), typeof(Serde.Test.JsonSerializerTests.DtWrap).GetProperty("Value"))
        }
        );
    }
}
