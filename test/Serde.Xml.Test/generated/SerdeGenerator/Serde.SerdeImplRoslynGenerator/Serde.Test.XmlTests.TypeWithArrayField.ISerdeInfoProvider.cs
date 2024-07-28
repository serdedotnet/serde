
#nullable enable
namespace Serde.Test;
partial class XmlTests
{
    partial class TypeWithArrayField : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "TypeWithArrayField",
        typeof(Serde.Test.XmlTests.TypeWithArrayField).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("ArrayField", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<Serde.Test.XmlTests.StructWithIntField,global::Serde.IdWrap<Serde.Test.XmlTests.StructWithIntField>>>(), typeof(Serde.Test.XmlTests.TypeWithArrayField).GetField("ArrayField")!)
    });
}
}