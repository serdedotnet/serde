
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct IdStructList
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "IdStructList",
        typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("count", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetProperty("Count")),
            ("list", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Generic.List<Serde.Test.JsonDeserializeTests.IdStruct>, Serde.ListProxy.De<Serde.Test.JsonDeserializeTests.IdStruct, Serde.Test.JsonDeserializeTests.IdStruct>>(), typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetProperty("List"))
        }
        );
    }
}
