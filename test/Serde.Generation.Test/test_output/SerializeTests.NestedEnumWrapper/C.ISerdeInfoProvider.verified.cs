//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial class C : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(C).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("colorOpt", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableWrap.SerializeImpl<Rgb,RgbWrap>>(), typeof(C).GetField("ColorOpt")!)
    });
}