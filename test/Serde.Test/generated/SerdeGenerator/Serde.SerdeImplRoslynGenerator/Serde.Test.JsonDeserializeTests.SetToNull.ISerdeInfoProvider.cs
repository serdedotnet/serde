
#nullable enable
namespace Serde.Test;
partial class JsonDeserializeTests
{
    partial record struct SetToNull : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "SetToNull",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("present", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.JsonDeserializeTests.SetToNull).GetProperty("Present")!),
("missing", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefWrap.DeserializeImpl<string,global::Serde.StringWrap>>(), typeof(Serde.Test.JsonDeserializeTests.SetToNull).GetProperty("Missing")!)
    });
}
}