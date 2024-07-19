
#nullable enable
namespace Serde.Test;
partial class JsonDeserializeTests
{
    partial record SkipDeserialize : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "SkipDeserialize",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("required", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetProperty("Required")!),
("skip", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetProperty("Skip")!)
    });
}
}