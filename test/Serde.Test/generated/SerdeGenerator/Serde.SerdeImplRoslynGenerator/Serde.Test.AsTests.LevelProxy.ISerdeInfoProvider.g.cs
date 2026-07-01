
#nullable enable

namespace Serde.Test;

partial class AsTests
{
    partial class LevelProxy : global::Serde.ISerdeInfoProvider
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
            "Level",
            typeof(Serde.Test.AsTests.Level).GetCustomAttributesData(),
            global::Serde.SerdeInfoProvider.GetSerializeInfo<byte, global::Serde.U8Proxy>(),
            new (string, System.Reflection.MemberInfo?)[] {
                ("a", typeof(Serde.Test.AsTests.Level).GetField("A")),
                ("b", typeof(Serde.Test.AsTests.Level).GetField("B"))
            }
        );
    }
}
