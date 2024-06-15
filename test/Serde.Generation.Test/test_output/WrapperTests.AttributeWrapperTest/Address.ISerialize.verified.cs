//HintName: Address.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class Address : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("Address", 5);
        type.SerializeField("name"u8, new StringWrap(this.Name));
        type.SerializeField("line1"u8, new StringWrap(this.Line1));
        type.SerializeField("city"u8, new StringWrap(this.City));
        type.SerializeField("state"u8, new StringWrap(this.State));
        type.SerializeField("zip"u8, new StringWrap(this.Zip));
        type.End();
    }
}