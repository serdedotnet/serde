
#nullable enable
namespace Serde.Test;
partial class XmlTests
{
    partial class MapTest1 : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "MapTest1",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("MapField", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictWrap.SerializeImpl<string,global::Serde.StringWrap,int,global::Serde.Int32Wrap>>(), typeof(Serde.Test.XmlTests.MapTest1).GetField("MapField")!)
    });
}
}