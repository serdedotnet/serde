//HintName: Some.Nested.Namespace.Base._m_AProxy.ISerdeInfoProvider.cs

#nullable enable
namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_AProxy : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "A",
            typeof(Some.Nested.Namespace.Base.A).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
                ("x", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Proxy>(), typeof(Some.Nested.Namespace.Base.A).GetProperty("X")!)
            }
        );
    }
}