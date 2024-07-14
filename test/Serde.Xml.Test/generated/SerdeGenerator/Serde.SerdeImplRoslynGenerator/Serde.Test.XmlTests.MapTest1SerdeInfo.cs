namespace Serde.Test;
partial class XmlTests
{
    internal static class MapTest1SerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "MapTest1",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("MapField", typeof(Serde.Test.XmlTests.MapTest1).GetField("MapField")!)
    });
}
}