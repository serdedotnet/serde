
#nullable enable
namespace Serde.Test;
partial class XmlTests
{
    partial class TypeWithArrayField : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "TypeWithArrayField",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("ArrayField", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<Serde.Test.XmlTests.StructWithIntField,global::Serde.IdWrap<Serde.Test.XmlTests.StructWithIntField>>>(), typeof(Serde.Test.XmlTests.TypeWithArrayField).GetField("ArrayField")!)
    });
}
}