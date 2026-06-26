//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(C).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("colorInt", global::Serde.SerdeInfoProvider.GetDeserializeInfo<ColorInt, ColorIntProxy>()),
            new("colorByte", global::Serde.SerdeInfoProvider.GetDeserializeInfo<ColorByte, ColorByteProxy>()),
            new("colorLong", global::Serde.SerdeInfoProvider.GetDeserializeInfo<ColorLong, ColorLongProxy>()),
            new("colorULong", global::Serde.SerdeInfoProvider.GetDeserializeInfo<ColorULong, ColorULongProxy>())
        }
    );
}
