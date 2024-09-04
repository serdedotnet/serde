
#nullable enable
namespace Serde.Json.Test;
partial class InvalidJsonTests
{
    partial class ClassWithParameterizedCtor_WithPoco : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "ClassWithParameterizedCtor_WithPoco",
        typeof(Serde.Json.Test.InvalidJsonTests.ClassWithParameterizedCtor_WithPoco).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("obj", global::Serde.SerdeInfoProvider.GetInfo<Serde.Json.Test.InvalidJsonTests.PocoWithParameterizedCtor>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithParameterizedCtor_WithPoco).GetProperty("Obj")!)
    });
}
}