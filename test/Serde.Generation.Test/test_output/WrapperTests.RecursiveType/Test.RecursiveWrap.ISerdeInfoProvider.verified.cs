//HintName: Test.RecursiveWrap.ISerdeInfoProvider.cs

#nullable enable
namespace Test;
partial record struct RecursiveWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "Recursive",
        typeof(Recursive).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("next", global::Serde.SerdeInfoProvider.GetInfo<RecursiveWrap>(), typeof(Recursive).GetProperty("Next")!)
    });
}