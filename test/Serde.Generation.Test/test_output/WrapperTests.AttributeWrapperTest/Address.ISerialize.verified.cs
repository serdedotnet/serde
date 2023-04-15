//HintName: Address.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class Address : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("Address", 5);
        type.SerializeField("name", new StringWrap(this.Name), new System.Attribute[] { new System.Xml.Serialization.XmlAttributeAttribute() { }, new Serde.SerdeMemberOptions() { ProvideAttributes = true } });
        type.SerializeField("line1", new StringWrap(this.Line1));
        type.SerializeField("city", new StringWrap(this.City));
        type.SerializeField("state", new StringWrap(this.State));
        type.SerializeField("zip", new StringWrap(this.Zip));
        type.End();
    }
}