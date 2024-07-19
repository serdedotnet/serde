//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial class C : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "C",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("s", global::Serde.SerdeInfoProvider.GetInfo<SWrap.SerializeImpl<int,global::Serde.Int32Wrap>>(), typeof(C).GetField("S")!)
    });
}