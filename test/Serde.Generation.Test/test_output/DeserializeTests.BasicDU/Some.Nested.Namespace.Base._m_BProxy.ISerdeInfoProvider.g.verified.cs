//HintName: Some.Nested.Namespace.Base._m_BProxy.ISerdeInfoProvider.g.cs

#nullable enable

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_BProxy
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "B",
        typeof(Some.Nested.Namespace.Base.B).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("y", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(Some.Nested.Namespace.Base.B).GetProperty("Y"))
        }
        );
    }
}
