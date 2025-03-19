
#nullable enable

namespace Serde.Test;

partial class XmlTests
{
    partial struct BoolStruct
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "BoolStruct",
        typeof(Serde.Test.XmlTests.BoolStruct).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("BoolField", global::Serde.SerdeInfoProvider.GetSerializeInfo<bool, global::Serde.BoolProxy>(), typeof(Serde.Test.XmlTests.BoolStruct).GetField("BoolField"))
        }
        );
    }
}