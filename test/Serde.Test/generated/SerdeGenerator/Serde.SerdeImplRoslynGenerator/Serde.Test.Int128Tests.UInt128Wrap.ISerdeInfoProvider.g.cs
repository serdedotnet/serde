
#nullable enable

namespace Serde.Test;

partial class Int128Tests
{
    partial record UInt128Wrap
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "UInt128Wrap",
        typeof(Serde.Test.Int128Tests.UInt128Wrap).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("value", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.UInt128, global::Serde.U128Proxy>(), typeof(Serde.Test.Int128Tests.UInt128Wrap).GetProperty("Value"))
        }
        );
    }
}
