
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct DenyUnknown : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "DenyUnknown",
            typeof(Serde.Test.JsonDeserializeTests.DenyUnknown).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
                ("present", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.DenyUnknown).GetProperty("Present")),
                ("missing", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefProxy.Deserialize<string, global::Serde.StringProxy>>(), typeof(Serde.Test.JsonDeserializeTests.DenyUnknown).GetProperty("Missing"))
            }
        );
    }
}