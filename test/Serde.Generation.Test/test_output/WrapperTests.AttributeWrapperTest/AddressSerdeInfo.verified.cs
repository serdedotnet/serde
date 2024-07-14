//HintName: AddressSerdeInfo.cs
internal static class AddressSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "Address",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("name", typeof(Address).GetField("Name")!),
("line1", typeof(Address).GetField("Line1")!),
("city", typeof(Address).GetField("City")!),
("state", typeof(Address).GetField("State")!),
("zip", typeof(Address).GetField("Zip")!)
    });
}