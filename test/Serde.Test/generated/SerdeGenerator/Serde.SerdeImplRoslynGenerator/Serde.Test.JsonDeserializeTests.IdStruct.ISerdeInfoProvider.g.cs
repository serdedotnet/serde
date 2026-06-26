
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial struct IdStruct
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "IdStruct",
            typeof(Serde.Test.JsonDeserializeTests.IdStruct).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("id", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
