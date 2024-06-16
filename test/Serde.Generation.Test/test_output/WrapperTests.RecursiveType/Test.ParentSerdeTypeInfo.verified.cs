//HintName: Test.ParentSerdeTypeInfo.cs
namespace Test;
internal static class ParentSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("r", typeof(Test.Parent).GetProperty("R")!)
    });
}