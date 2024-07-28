
#nullable enable
namespace Serde.Test;
partial class XmlTests
{
    partial class MapTest1 : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "MapTest1",
        typeof(Serde.Test.XmlTests.MapTest1).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("MapField", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictWrap.SerializeImpl<string,global::Serde.StringWrap,int,global::Serde.Int32Wrap>>(), typeof(Serde.Test.XmlTests.MapTest1).GetField("MapField")!)
    });
}
}