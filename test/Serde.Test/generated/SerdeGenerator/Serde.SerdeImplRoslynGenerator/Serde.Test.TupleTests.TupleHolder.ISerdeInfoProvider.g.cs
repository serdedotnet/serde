
#nullable enable

namespace Serde.Test;

partial class TupleTests
{
    partial record TupleHolder
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "TupleHolder",
            typeof(Serde.Test.TupleTests.TupleHolder).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("Pair", global::Serde.SerdeInfoProvider.GetSerializeInfo<(int, string), Serde.TupleProxy.Ser<int, string, global::Serde.I32Proxy, global::Serde.StringProxy>>()),
                new("Nested", global::Serde.SerdeInfoProvider.GetSerializeInfo<(int, (string, bool)), Serde.TupleProxy.Ser<int, (string, bool), global::Serde.I32Proxy, Serde.TupleProxy.Ser<string, bool, global::Serde.StringProxy, global::Serde.BoolProxy>>>()),
                new("Points", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Generic.List<(int, int)>, Serde.ListProxy.Ser<(int, int), Serde.TupleProxy.Ser<int, int, global::Serde.I32Proxy, global::Serde.I32Proxy>>>())
            }
        );
    }
}
