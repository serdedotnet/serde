//HintName: Test.Parent.ISerdeInfoProvider.cs

#nullable enable
namespace Test;
partial record Parent : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "Parent",
        typeof(Test.Parent).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("r", global::Serde.SerdeInfoProvider.GetInfo<Test.RecursiveWrap>(), typeof(Test.Parent).GetProperty("R")!)
    });
}