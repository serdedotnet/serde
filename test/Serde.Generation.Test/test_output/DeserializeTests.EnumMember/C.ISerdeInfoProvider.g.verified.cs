//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
    typeof(C).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("colorInt", global::Serde.SerdeInfoProvider.GetDeserializeInfo<ColorInt, ColorIntProxy>(), typeof(C).GetField("ColorInt")),
        ("colorByte", global::Serde.SerdeInfoProvider.GetDeserializeInfo<ColorByte, ColorByteProxy>(), typeof(C).GetField("ColorByte")),
        ("colorLong", global::Serde.SerdeInfoProvider.GetDeserializeInfo<ColorLong, ColorLongProxy>(), typeof(C).GetField("ColorLong")),
        ("colorULong", global::Serde.SerdeInfoProvider.GetDeserializeInfo<ColorULong, ColorULongProxy>(), typeof(C).GetField("ColorULong"))
    }
    );
}
