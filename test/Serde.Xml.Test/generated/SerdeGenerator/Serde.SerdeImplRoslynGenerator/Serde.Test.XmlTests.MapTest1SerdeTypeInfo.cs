namespace Serde.Test;
partial class XmlTests
{
    internal static class MapTest1SerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("MapField", typeof(MapTest1).GetField("MapField")!)
    });
}
}