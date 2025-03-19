
#nullable enable

namespace Serde.Test;

partial class XmlTests
{
    partial class MapTest1
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "MapTest1",
        typeof(Serde.Test.XmlTests.MapTest1).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("MapField", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Generic.Dictionary<string, int>, Serde.DictProxy.Ser<string, int, global::Serde.StringProxy, global::Serde.I32Proxy>>(), typeof(Serde.Test.XmlTests.MapTest1).GetField("MapField"))
        }
        );
    }
}