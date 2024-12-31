
#nullable enable
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct SetToNull : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "SetToNull",
            typeof(Serde.Test.JsonDeserializeTests.SetToNull).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
                ("present", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringProxy>(), typeof(Serde.Test.JsonDeserializeTests.SetToNull).GetProperty("Present")!),
                ("missing", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefProxy.Deserialize<string,global::Serde.StringProxy>>(), typeof(Serde.Test.JsonDeserializeTests.SetToNull).GetProperty("Missing")!)
            }
        );
    }
}