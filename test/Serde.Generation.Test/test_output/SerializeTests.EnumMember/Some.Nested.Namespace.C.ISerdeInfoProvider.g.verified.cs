//HintName: Some.Nested.Namespace.C.ISerdeInfoProvider.g.cs

#nullable enable

namespace Some.Nested.Namespace;

partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
    typeof(Some.Nested.Namespace.C).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("colorInt", global::Serde.SerdeInfoProvider.GetSerializeInfo<Some.Nested.Namespace.ColorInt, Some.Nested.Namespace.ColorIntProxy>(), typeof(Some.Nested.Namespace.C).GetField("ColorInt")),
        ("colorByte", global::Serde.SerdeInfoProvider.GetSerializeInfo<Some.Nested.Namespace.ColorByte, Some.Nested.Namespace.ColorByteProxy>(), typeof(Some.Nested.Namespace.C).GetField("ColorByte")),
        ("colorLong", global::Serde.SerdeInfoProvider.GetSerializeInfo<Some.Nested.Namespace.ColorLong, Some.Nested.Namespace.ColorLongProxy>(), typeof(Some.Nested.Namespace.C).GetField("ColorLong")),
        ("colorULong", global::Serde.SerdeInfoProvider.GetSerializeInfo<Some.Nested.Namespace.ColorULong, Some.Nested.Namespace.ColorULongProxy>(), typeof(Some.Nested.Namespace.C).GetField("ColorULong"))
    }
    );
}
