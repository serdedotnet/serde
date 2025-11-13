
#nullable enable

namespace Serde.Test;

partial class Int128Tests
{
    partial record Int128Wrap
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Int128Wrap",
        typeof(Serde.Test.Int128Tests.Int128Wrap).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("value", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Int128, global::Serde.I128Proxy>(), typeof(Serde.Test.Int128Tests.Int128Wrap).GetProperty("Value"))
        }
        );
    }
}
