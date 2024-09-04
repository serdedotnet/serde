
#nullable enable
namespace Serde.Json.Test;
partial class InvalidJsonTests
{
    partial class SkipClass : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "SkipClass",
        typeof(Serde.Json.Test.InvalidJsonTests.SkipClass).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("c", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Json.Test.InvalidJsonTests.SkipClass).GetProperty("C")!)
    });
}
}