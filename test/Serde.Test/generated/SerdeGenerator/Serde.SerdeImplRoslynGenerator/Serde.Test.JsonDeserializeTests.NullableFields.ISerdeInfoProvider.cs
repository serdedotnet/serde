
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial class NullableFields : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "NullableFields",
            typeof(Serde.Test.JsonDeserializeTests.NullableFields).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
                ("s", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(), typeof(Serde.Test.JsonDeserializeTests.NullableFields).GetField("S")),
                ("dict", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictProxy.De<string, string?, global::Serde.StringProxy, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>>(), typeof(Serde.Test.JsonDeserializeTests.NullableFields).GetField("Dict"))
            }
        );
    }
}