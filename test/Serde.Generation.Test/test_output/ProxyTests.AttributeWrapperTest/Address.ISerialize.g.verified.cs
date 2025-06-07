//HintName: Address.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial class Address
{
    sealed partial class _SerObj : Serde.ISerialize<Address>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Address.s_serdeInfo;

        void global::Serde.ISerialize<Address>.Serialize(Address value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteString(_l_info, 0, value.Name);
            _l_type.WriteString(_l_info, 1, value.Line1);
            _l_type.WriteString(_l_info, 2, value.City);
            _l_type.WriteString(_l_info, 3, value.State);
            _l_type.WriteString(_l_info, 4, value.Zip);
            _l_type.End(_l_info);
        }

    }
}
