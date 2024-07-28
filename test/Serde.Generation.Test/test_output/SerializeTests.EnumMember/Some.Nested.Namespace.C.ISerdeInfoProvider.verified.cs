//HintName: Some.Nested.Namespace.C.ISerdeInfoProvider.cs

#nullable enable
namespace Some.Nested.Namespace;
partial class C : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(Some.Nested.Namespace.C).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("colorInt", global::Serde.SerdeInfoProvider.GetInfo<Some.Nested.Namespace.ColorIntWrap>(), typeof(Some.Nested.Namespace.C).GetField("ColorInt")!),
("colorByte", global::Serde.SerdeInfoProvider.GetInfo<Some.Nested.Namespace.ColorByteWrap>(), typeof(Some.Nested.Namespace.C).GetField("ColorByte")!),
("colorLong", global::Serde.SerdeInfoProvider.GetInfo<Some.Nested.Namespace.ColorLongWrap>(), typeof(Some.Nested.Namespace.C).GetField("ColorLong")!),
("colorULong", global::Serde.SerdeInfoProvider.GetInfo<Some.Nested.Namespace.ColorULongWrap>(), typeof(Some.Nested.Namespace.C).GetField("ColorULong")!)
    });
}