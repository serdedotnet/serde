//HintName: Address.ISerdeInfoProvider.cs

#nullable enable
partial class Address : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "Address",
        typeof(Address).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("name", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Address).GetField("Name")!),
("line1", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Address).GetField("Line1")!),
("city", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Address).GetField("City")!),
("state", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Address).GetField("State")!),
("zip", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Address).GetField("Zip")!)
    });
}