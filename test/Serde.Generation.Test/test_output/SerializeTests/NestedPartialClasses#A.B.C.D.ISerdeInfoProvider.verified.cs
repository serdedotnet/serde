//HintName: A.B.C.D.ISerdeInfoProvider.cs

#nullable enable
partial class A
{
    partial class B
{
    partial class C
{
    partial class D : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "D",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("field", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(A.B.C.D).GetField("Field")!)
    });
}
}
}
}