//HintName: Address.ISerialize.cs

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
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Address>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<string,global::Serde.StringProxy>(_l_serdeInfo,0,value.Name);
            type.SerializeField<string,global::Serde.StringProxy>(_l_serdeInfo,1,value.Line1);
            type.SerializeField<string,global::Serde.StringProxy>(_l_serdeInfo,2,value.City);
            type.SerializeField<string,global::Serde.StringProxy>(_l_serdeInfo,3,value.State);
            type.SerializeField<string,global::Serde.StringProxy>(_l_serdeInfo,4,value.Zip);
            type.End();
        }
        public static readonly AddressSerializeProxy Instance = new();
        private AddressSerializeProxy() { }

    }
}
