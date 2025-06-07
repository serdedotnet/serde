
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial struct IdStruct
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "IdStruct",
        typeof(Serde.Test.JsonDeserializeTests.IdStruct).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("id", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.JsonDeserializeTests.IdStruct).GetField("Id"))
        }
        );
    }
}
