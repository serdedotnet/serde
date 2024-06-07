//HintName: Test.ParentSerdeTypeInfo.cs
namespace Test;
internal static class ParentSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("r", typeof(Parent).GetProperty("R")!)
    });
}