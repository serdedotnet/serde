
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct IdStructList
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "IdStructList",
            typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("count", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>()),
                new("list", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Generic.List<Serde.Test.JsonDeserializeTests.IdStruct>, Serde.ListProxy.De<Serde.Test.JsonDeserializeTests.IdStruct, Serde.Test.JsonDeserializeTests.IdStruct>>())
            }
        );
    }
}
