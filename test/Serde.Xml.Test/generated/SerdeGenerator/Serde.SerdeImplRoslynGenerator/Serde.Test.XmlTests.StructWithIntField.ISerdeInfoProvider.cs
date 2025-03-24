
#nullable enable

namespace Serde.Test;

partial class XmlTests
{
    partial record StructWithIntField
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "StructWithIntField",
        typeof(Serde.Test.XmlTests.StructWithIntField).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("X", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.XmlTests.StructWithIntField).GetProperty("X"))
        }
        );
    }
}