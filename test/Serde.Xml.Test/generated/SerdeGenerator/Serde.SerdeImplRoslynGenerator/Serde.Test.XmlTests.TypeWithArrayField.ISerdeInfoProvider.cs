
#nullable enable

namespace Serde.Test;

partial class XmlTests
{
    partial class TypeWithArrayField
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "TypeWithArrayField",
        typeof(Serde.Test.XmlTests.TypeWithArrayField).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("ArrayField", global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.XmlTests.StructWithIntField[], Serde.ArrayProxy.Ser<Serde.Test.XmlTests.StructWithIntField, Serde.Test.XmlTests.StructWithIntField>>(), typeof(Serde.Test.XmlTests.TypeWithArrayField).GetField("ArrayField"))
        }
        );
    }
}