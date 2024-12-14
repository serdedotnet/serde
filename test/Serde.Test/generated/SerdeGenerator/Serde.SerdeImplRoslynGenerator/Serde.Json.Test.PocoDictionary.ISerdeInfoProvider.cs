
#nullable enable
namespace Serde.Json.Test;
partial class PocoDictionary : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "PocoDictionary",
        typeof(Serde.Json.Test.PocoDictionary).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("key", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictProxy.Deserialize<string,string,global::Serde.StringProxy,global::Serde.StringProxy>>(), typeof(Serde.Json.Test.PocoDictionary).GetProperty("key")!)
    });
}