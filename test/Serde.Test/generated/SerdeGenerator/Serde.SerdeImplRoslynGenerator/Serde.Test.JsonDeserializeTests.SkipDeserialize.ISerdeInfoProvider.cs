
#nullable enable
namespace Serde.Test;
partial class JsonDeserializeTests
{
    partial record SkipDeserialize : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "SkipDeserialize",
        typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("required", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetProperty("Required")!),
("skip", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.JsonDeserializeTests.SkipDeserialize).GetProperty("Skip")!)
    });
}
}