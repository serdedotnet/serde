
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial class NullableFields : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "NullableFields",
            typeof(Serde.Test.JsonSerializerTests.NullableFields).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
                ("s", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>(), typeof(Serde.Test.JsonSerializerTests.NullableFields).GetField("S")),
                ("d", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictProxy.Ser<string, string?, global::Serde.StringProxy, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>>(), typeof(Serde.Test.JsonSerializerTests.NullableFields).GetField("D"))
            }
        );
    }
}