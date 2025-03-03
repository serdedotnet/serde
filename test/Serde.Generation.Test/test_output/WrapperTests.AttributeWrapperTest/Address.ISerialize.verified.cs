﻿//HintName: Address.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class Address : Serde.ISerializeProvider<Address>
{
    static ISerialize<Address> ISerializeProvider<Address>.SerializeInstance
        => AddressSerializeProxy.Instance;

    sealed partial class AddressSerializeProxy :Serde.ISerialize<Address>
    {
        void global::Serde.ISerialize<Address>.Serialize(Address value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<Address>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteString(_l_info, 0, value.Name);
            _l_type.WriteString(_l_info, 1, value.Line1);
            _l_type.WriteString(_l_info, 2, value.City);
            _l_type.WriteString(_l_info, 3, value.State);
            _l_type.WriteString(_l_info, 4, value.Zip);
            _l_type.End(_l_info);
        }
        public static readonly AddressSerializeProxy Instance = new();
        private AddressSerializeProxy() { }

    }
}
