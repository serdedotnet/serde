//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial class C : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "C",
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("s", global::Serde.SerdeInfoProvider.GetInfo<OPTSWrap>(), typeof(C).GetField("S")!)
    });
}