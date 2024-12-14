//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial class C : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(C).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("colorInt", global::Serde.SerdeInfoProvider.GetInfo<ColorIntProxy>(), typeof(C).GetField("ColorInt")!),
("colorByte", global::Serde.SerdeInfoProvider.GetInfo<ColorByteProxy>(), typeof(C).GetField("ColorByte")!),
("colorLong", global::Serde.SerdeInfoProvider.GetInfo<ColorLongProxy>(), typeof(C).GetField("ColorLong")!),
("colorULong", global::Serde.SerdeInfoProvider.GetInfo<ColorULongProxy>(), typeof(C).GetField("ColorULong")!)
    });
}