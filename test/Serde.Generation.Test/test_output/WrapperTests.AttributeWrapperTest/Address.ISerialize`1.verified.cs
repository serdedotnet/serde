//HintName: Address.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial class Address : Serde.ISerialize<Address>
{
    void ISerialize<Address>.Serialize(Address value, ISerializer serializer)
    {
        var type = serializer.SerializeType("Address", 5);
        type.SerializeField<string, StringWrap>("name", value.Name, new System.Attribute[] { new System.Xml.Serialization.XmlAttributeAttribute() { }, new Serde.SerdeMemberOptions() { ProvideAttributes = true } });
        type.SerializeField<string, StringWrap>("line1", value.Line1);
        type.SerializeField<string, StringWrap>("city", value.City);
        type.SerializeField<string, StringWrap>("state", value.State);
        type.SerializeField<string, StringWrap>("zip", value.Zip);
        type.End();
    }
}