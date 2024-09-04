
#nullable enable
namespace Serde.Json.Test;
partial class InvalidJsonTests
{
    partial class ClassWithDictionaryOfPoco : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "ClassWithDictionaryOfPoco",
        typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("obj", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictWrap.DeserializeImpl<string,global::Serde.StringWrap,Serde.Json.Test.Poco,Serde.Json.Test.Poco>>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco).GetProperty("Obj")!)
    });
}
}