
#nullable enable
namespace Serde.Json.Test;
partial class InvalidJsonTests
{
    partial class ClassWithPocoArray : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "ClassWithPocoArray",
        typeof(Serde.Json.Test.InvalidJsonTests.ClassWithPocoArray).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("obj", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.DeserializeImpl<Serde.Json.Test.Poco,Serde.Json.Test.Poco>>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithPocoArray).GetProperty("Obj")!)
    });
}
}