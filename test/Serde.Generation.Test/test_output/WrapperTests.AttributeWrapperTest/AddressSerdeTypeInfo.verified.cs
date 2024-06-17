//HintName: AddressSerdeTypeInfo.cs
internal static class AddressSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("name", typeof(Address).GetField("Name")!),
("line1", typeof(Address).GetField("Line1")!),
("city", typeof(Address).GetField("City")!),
("state", typeof(Address).GetField("State")!),
("zip", typeof(Address).GetField("Zip")!)
    });
}