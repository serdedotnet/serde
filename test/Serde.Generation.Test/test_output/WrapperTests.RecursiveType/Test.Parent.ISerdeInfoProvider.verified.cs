//HintName: Test.Parent.ISerdeInfoProvider.cs

#nullable enable
namespace Test;
partial record Parent : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "Parent",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("r", global::Serde.SerdeInfoProvider.GetInfo<Test.RecursiveWrap>(), typeof(Test.Parent).GetProperty("R")!)
    });
}