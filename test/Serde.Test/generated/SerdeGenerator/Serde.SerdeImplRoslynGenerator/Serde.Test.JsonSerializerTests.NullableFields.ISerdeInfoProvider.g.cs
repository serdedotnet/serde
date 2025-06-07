
#nullable enable

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial class NullableFields
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "NullableFields",
        typeof(Serde.Test.JsonSerializerTests.NullableFields).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("s", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>(), typeof(Serde.Test.JsonSerializerTests.NullableFields).GetField("S")),
            ("d", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Generic.Dictionary<string, string?>, Serde.DictProxy.Ser<string, string?, global::Serde.StringProxy, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>>(), typeof(Serde.Test.JsonSerializerTests.NullableFields).GetField("D"))
        }
        );
    }
}
