
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct ThrowMissing : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "ThrowMissing",
            typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
                ("present", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetProperty("Present")),
                ("missing", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(), typeof(Serde.Test.JsonDeserializeTests.ThrowMissing).GetProperty("Missing"))
            }
        );
    }
}