
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record DtoWrap
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "DtoWrap",
            typeof(Serde.Test.JsonSerializerTests.DtoWrap).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("value", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.DateTimeOffset, global::Serde.DateTimeOffsetProxy>())
            }
        );
    }
}
