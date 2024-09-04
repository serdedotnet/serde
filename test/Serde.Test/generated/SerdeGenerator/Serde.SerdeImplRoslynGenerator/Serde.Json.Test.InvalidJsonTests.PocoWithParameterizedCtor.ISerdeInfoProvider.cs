
#nullable enable
namespace Serde.Json.Test;
partial class InvalidJsonTests
{
    partial class PocoWithParameterizedCtor : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "PocoWithParameterizedCtor",
        typeof(Serde.Json.Test.InvalidJsonTests.PocoWithParameterizedCtor).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("obj", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Json.Test.InvalidJsonTests.PocoWithParameterizedCtor).GetProperty("Obj")!)
    });
}
}