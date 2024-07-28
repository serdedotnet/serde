
#nullable enable
namespace Serde.Test;
partial class JsonDeserializeTests
{
    partial struct IdStruct : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "IdStruct",
        typeof(Serde.Test.JsonDeserializeTests.IdStruct).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("id", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Test.JsonDeserializeTests.IdStruct).GetField("Id")!)
    });
}
}