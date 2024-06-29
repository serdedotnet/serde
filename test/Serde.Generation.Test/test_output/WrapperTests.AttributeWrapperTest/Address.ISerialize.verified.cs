//HintName: Address.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class Address : Serde.ISerialize<Address>
{
    void ISerialize<Address>.Serialize(Address value, ISerializer serializer)
    {
        var _l_typeInfo = AddressSerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeField<string, StringWrap>(_l_typeInfo, 0, this.Name);
        type.SerializeField<string, StringWrap>(_l_typeInfo, 1, this.Line1);
        type.SerializeField<string, StringWrap>(_l_typeInfo, 2, this.City);
        type.SerializeField<string, StringWrap>(_l_typeInfo, 3, this.State);
        type.SerializeField<string, StringWrap>(_l_typeInfo, 4, this.Zip);
        type.End();
    }
}