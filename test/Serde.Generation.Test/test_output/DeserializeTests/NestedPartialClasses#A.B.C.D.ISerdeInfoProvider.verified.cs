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
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "D",
        typeof(A.B.C.D).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("field", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(A.B.C.D).GetField("Field")!)
    });
}
}
}
}