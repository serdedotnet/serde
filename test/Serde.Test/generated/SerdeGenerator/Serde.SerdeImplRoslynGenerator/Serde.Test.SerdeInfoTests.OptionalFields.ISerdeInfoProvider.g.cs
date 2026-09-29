
#nullable enable

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record OptionalFields
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "OptionalFields",
            typeof(Serde.Test.SerdeInfoTests.OptionalFields).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("requiredReference", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>()),
                new("requiredValue", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
                new("nullableReference", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>())
                {
                    IsOptional = true,
                },
                new("nullableValue", global::Serde.SerdeInfoProvider.GetSerializeInfo<int?, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>())
                {
                    IsOptional = true,
                },
                new("requiredNullableReference", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>())
                {
                    MemberInfo = typeof(Serde.Test.SerdeInfoTests.OptionalFields).GetProperty("RequiredNullableReference"),
                },
                new("requiredNullableValue", global::Serde.SerdeInfoProvider.GetSerializeInfo<int?, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>())
                {
                    MemberInfo = typeof(Serde.Test.SerdeInfoTests.OptionalFields).GetProperty("RequiredNullableValue"),
                },
                new("optionalReference", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
                {
                    MemberInfo = typeof(Serde.Test.SerdeInfoTests.OptionalFields).GetProperty("OptionalReference"),
                    IsOptional = true,
                },
                new("optionalValue", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.SerdeInfoTests.OptionalFields).GetProperty("OptionalValue"),
                    IsOptional = true,
                },
                new("initializedReference", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
                {
                    IsOptional = true,
                },
                new("initializedValue", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    IsOptional = true,
                },
                new("staticInitializer", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
                {
                    IsOptional = true,
                },
                new("unsupportedInitializer", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>()),
                new("requiredInitializer", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.SerdeInfoTests.OptionalFields).GetProperty("RequiredInitializer"),
                },
                new("skipped", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
                {
                    MemberInfo = typeof(Serde.Test.SerdeInfoTests.OptionalFields).GetProperty("Skipped"),
                    IsOptional = true,
                }
            }
        );
    }
}
