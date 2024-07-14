//HintName: Test.ParentSerdeInfo.cs
namespace Test;
internal static class ParentSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "Parent",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("r", typeof(Test.Parent).GetProperty("R")!)
    });
}