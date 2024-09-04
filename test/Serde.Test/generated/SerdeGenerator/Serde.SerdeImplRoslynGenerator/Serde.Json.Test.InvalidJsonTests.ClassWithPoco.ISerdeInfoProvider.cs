
#nullable enable
namespace Serde.Json.Test;
partial class InvalidJsonTests
{
    partial class ClassWithPoco : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "ClassWithPoco",
        typeof(Serde.Json.Test.InvalidJsonTests.ClassWithPoco).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("obj", global::Serde.SerdeInfoProvider.GetInfo<Serde.Json.Test.Poco>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithPoco).GetProperty("Obj")!)
    });
}
}