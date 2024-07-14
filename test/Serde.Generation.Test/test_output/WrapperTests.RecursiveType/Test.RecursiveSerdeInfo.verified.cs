//HintName: Test.RecursiveSerdeInfo.cs
namespace Test;
internal static class RecursiveSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "Recursive",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("next", typeof(Recursive).GetProperty("Next")!)
    });
}