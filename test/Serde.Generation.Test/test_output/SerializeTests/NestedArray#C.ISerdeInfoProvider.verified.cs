//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial class C : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "C",
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("nestedArr", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<int[],Serde.ArrayWrap.SerializeImpl<int,global::Serde.Int32Wrap>>>(), typeof(C).GetField("NestedArr")!)
    });
}