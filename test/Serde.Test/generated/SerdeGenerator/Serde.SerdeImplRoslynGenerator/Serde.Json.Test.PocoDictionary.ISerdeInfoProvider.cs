
#nullable enable
namespace Serde.Json.Test;
partial class PocoDictionary : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "PocoDictionary",
        typeof(Serde.Json.Test.PocoDictionary).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("key", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictWrap.DeserializeImpl<string,global::Serde.StringWrap,string,global::Serde.StringWrap>>(), typeof(Serde.Json.Test.PocoDictionary).GetProperty("key")!)
    });
}