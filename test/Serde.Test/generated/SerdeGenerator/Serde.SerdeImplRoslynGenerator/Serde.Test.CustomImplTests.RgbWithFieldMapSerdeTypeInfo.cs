namespace Serde.Test;
partial class CustomImplTests
{
    internal static class RgbWithFieldMapSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("red", typeof(RgbWithFieldMap).GetField("Red")!),
("green", typeof(RgbWithFieldMap).GetField("Green")!),
("blue", typeof(RgbWithFieldMap).GetField("Blue")!)
    });
}
}