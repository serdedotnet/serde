//HintName: Some.Nested.Namespace.C.ISerdeInfoProvider.g.cs

#nullable enable

namespace Some.Nested.Namespace;

partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(Some.Nested.Namespace.C).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("colorInt", global::Serde.SerdeInfoProvider.GetSerializeInfo<Some.Nested.Namespace.ColorInt, Some.Nested.Namespace.ColorIntProxy>()),
            new("colorByte", global::Serde.SerdeInfoProvider.GetSerializeInfo<Some.Nested.Namespace.ColorByte, Some.Nested.Namespace.ColorByteProxy>()),
            new("colorLong", global::Serde.SerdeInfoProvider.GetSerializeInfo<Some.Nested.Namespace.ColorLong, Some.Nested.Namespace.ColorLongProxy>()),
            new("colorULong", global::Serde.SerdeInfoProvider.GetSerializeInfo<Some.Nested.Namespace.ColorULong, Some.Nested.Namespace.ColorULongProxy>())
        }
    );
}
