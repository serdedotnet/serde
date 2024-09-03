
#nullable enable
namespace Serde.Json.Test;
partial class InvalidJsonTests
{
    partial class ClassWithDictionaryOfPocoList : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "ClassWithDictionaryOfPocoList",
        typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPocoList).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("obj", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictWrap.DeserializeImpl<string,global::Serde.StringWrap,System.Collections.Generic.List<Serde.Json.Test.Poco>,Serde.ListWrap.DeserializeImpl<Serde.Json.Test.Poco,Serde.Json.Test.Poco>>>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPocoList).GetProperty("Obj")!)
    });
}
}