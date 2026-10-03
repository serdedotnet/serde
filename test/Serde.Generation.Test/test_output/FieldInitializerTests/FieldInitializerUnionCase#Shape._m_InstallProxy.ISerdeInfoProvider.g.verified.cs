//HintName: Shape._m_InstallProxy.ISerdeInfoProvider.g.cs

#nullable enable
partial record Shape
{
    partial class _m_InstallProxy
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Install",
            typeof(Shape.Install).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("flag", global::Serde.SerdeInfoProvider.GetDeserializeInfo<bool, global::Serde.BoolProxy>())
                {
                    IsOptional = true,
                },
                new("fromPrivateConst", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
                {
                    IsOptional = true,
                },
                new("fromPrivate", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>()),
                new("fromInternal", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
                {
                    IsOptional = true,
                }
            }
        );
    }
}
