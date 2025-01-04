//HintName: Some.Nested.Namespace.Base._m_BProxy.ISerdeInfoProvider.cs

#nullable enable

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_BProxy : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "B",
            typeof(Some.Nested.Namespace.Base.B).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
                ("y", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringProxy>(), typeof(Some.Nested.Namespace.Base.B).GetProperty("Y")!)
            }
        );
    }
}