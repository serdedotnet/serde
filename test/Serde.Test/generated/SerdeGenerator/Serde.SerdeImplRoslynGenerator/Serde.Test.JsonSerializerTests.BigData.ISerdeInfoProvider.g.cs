
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BigData
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "BigData",
            typeof(Serde.Test.JsonSerializerTests.BigData).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("values", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Generic.List<int>, Serde.ListProxy.Ser<int, global::Serde.I32Proxy>>())
            }
        );
    }
}
