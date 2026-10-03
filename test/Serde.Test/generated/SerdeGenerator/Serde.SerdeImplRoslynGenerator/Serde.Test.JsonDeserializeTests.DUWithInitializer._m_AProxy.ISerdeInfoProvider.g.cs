
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record DUWithInitializer
    {
        partial class _m_AProxy
        {
            private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
                "A",
                typeof(Serde.Test.JsonDeserializeTests.DUWithInitializer.A).GetCustomAttributesData(),
                new global::Serde.SerdeInfo.FieldInfo[] {
                    new("name", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>()),
                    new("flag", global::Serde.SerdeInfoProvider.GetDeserializeInfo<bool, global::Serde.BoolProxy>())
                    {
                        IsOptional = true,
                    }
                }
            );
        }
    }
}
