
#nullable enable

namespace Serde.Test;

partial class XmlTests
{
    partial class NestedArrays
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "NestedArrays",
        typeof(Serde.Test.XmlTests.NestedArrays).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int[][][], Serde.ArrayProxy.Ser<int[][], Serde.ArrayProxy.Ser<int[], Serde.ArrayProxy.Ser<int, global::Serde.I32Proxy>>>>(), typeof(Serde.Test.XmlTests.NestedArrays).GetField("A"))
        }
        );
    }
}