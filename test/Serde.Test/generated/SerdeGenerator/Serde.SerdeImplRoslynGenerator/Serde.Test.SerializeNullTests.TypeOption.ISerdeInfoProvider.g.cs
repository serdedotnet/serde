
#nullable enable

namespace Serde.Test;

partial class SerializeNullTests
{
    partial class TypeOption
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "TypeOption",
            typeof(Serde.Test.SerializeNullTests.TypeOption).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("value", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>())
                {
                    IsOptional = true,
                }
            }
        );
    }
}
