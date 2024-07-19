
#nullable enable
namespace Serde.Test;
partial class JsonDeserializeTests
{
    partial struct ExtraMembers : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "ExtraMembers",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("b", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Test.JsonDeserializeTests.ExtraMembers).GetField("b")!)
    });
}
}