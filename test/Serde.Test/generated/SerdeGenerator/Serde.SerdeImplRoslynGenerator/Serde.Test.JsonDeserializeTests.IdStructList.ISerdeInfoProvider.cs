
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct IdStructList : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "IdStructList",
            typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
                ("count", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.I32Proxy>(), typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetProperty("Count")),
                ("list", global::Serde.SerdeInfoProvider.GetInfo<Serde.ListProxy.De<Serde.Test.JsonDeserializeTests.IdStruct, Serde.Test.JsonDeserializeTests.IdStruct>>(), typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetProperty("List"))
            }
        );
    }
}