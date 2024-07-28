
#nullable enable
namespace Serde.Test;
partial class JsonDeserializeTests
{
    partial struct ExtraMembers : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "ExtraMembers",
        typeof(Serde.Test.JsonDeserializeTests.ExtraMembers).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("b", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Test.JsonDeserializeTests.ExtraMembers).GetField("b")!)
    });
}
}