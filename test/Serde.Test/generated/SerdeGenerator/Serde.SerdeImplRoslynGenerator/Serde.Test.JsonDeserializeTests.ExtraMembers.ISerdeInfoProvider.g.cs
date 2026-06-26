
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial struct ExtraMembers
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "ExtraMembers",
            typeof(Serde.Test.JsonDeserializeTests.ExtraMembers).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("b", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
