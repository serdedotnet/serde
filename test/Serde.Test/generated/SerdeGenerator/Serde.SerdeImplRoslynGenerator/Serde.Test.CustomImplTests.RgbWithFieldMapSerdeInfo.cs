namespace Serde.Test;
partial class CustomImplTests
{
    internal static class RgbWithFieldMapSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "RgbWithFieldMap",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetField("Red")!),
("green", typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetField("Green")!),
("blue", typeof(Serde.Test.CustomImplTests.RgbWithFieldMap).GetField("Blue")!)
    });
}
}