//HintName: Test.RecursiveSerdeTypeInfo.cs
namespace Test;
internal static class RecursiveSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("next", typeof(Recursive).GetProperty("Next")!)
    });
}