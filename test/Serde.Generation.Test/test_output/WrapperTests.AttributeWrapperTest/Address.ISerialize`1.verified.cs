//HintName: Address.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial class Address : Serde.ISerialize<Address>
{
    void ISerialize<Address>.Serialize(Address value, ISerializer serializer)
    {
        var _l_typeInfo = AddressSerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeField<string, StringWrap>(_l_typeInfo, 0, value.Name);
        type.SerializeField<string, StringWrap>(_l_typeInfo, 1, value.Line1);
        type.SerializeField<string, StringWrap>(_l_typeInfo, 2, value.City);
        type.SerializeField<string, StringWrap>(_l_typeInfo, 3, value.State);
        type.SerializeField<string, StringWrap>(_l_typeInfo, 4, value.Zip);
        type.End();
    }
}