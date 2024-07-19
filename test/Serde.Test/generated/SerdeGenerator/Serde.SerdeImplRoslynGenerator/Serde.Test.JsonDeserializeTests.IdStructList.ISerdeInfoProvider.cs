
#nullable enable
namespace Serde.Test;
partial class JsonDeserializeTests
{
    partial record struct IdStructList : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "IdStructList",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("count", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetProperty("Count")!),
("list", global::Serde.SerdeInfoProvider.GetInfo<Serde.ListWrap.DeserializeImpl<Serde.Test.JsonDeserializeTests.IdStruct,Serde.Test.JsonDeserializeTests.IdStruct>>(), typeof(Serde.Test.JsonDeserializeTests.IdStructList).GetProperty("List")!)
    });
}
}