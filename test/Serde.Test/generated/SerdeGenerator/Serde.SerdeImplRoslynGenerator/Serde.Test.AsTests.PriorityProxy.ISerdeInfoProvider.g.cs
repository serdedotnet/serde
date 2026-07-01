
#nullable enable

namespace Serde.Test;

partial class AsTests
{
    partial class PriorityProxy : global::Serde.ISerdeInfoProvider
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
            "Priority",
            typeof(Serde.Test.AsTests.Priority).GetCustomAttributesData(),
            global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(),
            new (string, System.Reflection.MemberInfo?)[] {
                ("low", typeof(Serde.Test.AsTests.Priority).GetField("Low")),
                ("medium", typeof(Serde.Test.AsTests.Priority).GetField("Medium")),
                ("high", typeof(Serde.Test.AsTests.Priority).GetField("High"))
            }
        );
    }
}
