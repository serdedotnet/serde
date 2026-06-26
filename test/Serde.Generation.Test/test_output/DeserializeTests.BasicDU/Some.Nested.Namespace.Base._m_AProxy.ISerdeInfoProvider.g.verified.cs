//HintName: Some.Nested.Namespace.Base._m_AProxy.ISerdeInfoProvider.g.cs

#nullable enable

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_AProxy
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "A",
            typeof(Some.Nested.Namespace.Base.A).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("x", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
            }
        );
    }
}
