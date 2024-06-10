namespace Serde.Test;
partial class CustomImplTests
{
    internal static class RgbWithFieldMapSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetField("Red")!),
("green", typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetField("Green")!),
("blue", typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetField("Blue")!)
    });
}
}