//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial class C : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(C).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("colorInt", global::Serde.SerdeInfoProvider.GetInfo<ColorIntWrap>(), typeof(C).GetField("ColorInt")!),
("colorByte", global::Serde.SerdeInfoProvider.GetInfo<ColorByteWrap>(), typeof(C).GetField("ColorByte")!),
("colorLong", global::Serde.SerdeInfoProvider.GetInfo<ColorLongWrap>(), typeof(C).GetField("ColorLong")!),
("colorULong", global::Serde.SerdeInfoProvider.GetInfo<ColorULongWrap>(), typeof(C).GetField("ColorULong")!)
    });
}