
#nullable enable
namespace Serde.Json.Test;
partial class InvalidJsonTests
{
    partial class SkipDoubleCommaClass : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "SkipDoubleCommaClass",
        typeof(Serde.Json.Test.InvalidJsonTests.SkipDoubleCommaClass).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("c", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Json.Test.InvalidJsonTests.SkipDoubleCommaClass).GetProperty("C")!)
    });
}
}