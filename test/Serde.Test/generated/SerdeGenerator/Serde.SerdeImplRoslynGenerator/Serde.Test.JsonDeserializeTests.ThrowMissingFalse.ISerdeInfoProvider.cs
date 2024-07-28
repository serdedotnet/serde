
#nullable enable
namespace Serde.Test;
partial class JsonDeserializeTests
{
    partial record ThrowMissingFalse : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "ThrowMissingFalse",
        typeof(Serde.Test.JsonDeserializeTests.ThrowMissingFalse).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("present", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.JsonDeserializeTests.ThrowMissingFalse).GetProperty("Present")!),
("missing", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.BoolWrap>(), typeof(Serde.Test.JsonDeserializeTests.ThrowMissingFalse).GetProperty("Missing")!)
    });
}
}