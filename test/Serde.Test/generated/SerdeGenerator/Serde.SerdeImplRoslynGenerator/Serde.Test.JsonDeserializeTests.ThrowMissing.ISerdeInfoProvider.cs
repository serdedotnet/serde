
#nullable enable
namespace Serde.Test;
partial class JsonDeserializeTests
{
    partial record struct ThrowMissing : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "ThrowMissing",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("present", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetProperty("Present")!),
("missing", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefWrap.DeserializeImpl<string,global::Serde.StringWrap>>(), typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetProperty("Missing")!)
    });
}
}