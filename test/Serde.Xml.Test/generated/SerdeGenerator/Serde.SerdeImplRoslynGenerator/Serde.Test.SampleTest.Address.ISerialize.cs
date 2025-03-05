
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SampleTest
{
    partial record Address : Serde.ISerializeProvider<Serde.Test.SampleTest.Address>
    {
        static ISerialize<Serde.Test.SampleTest.Address> ISerializeProvider<Serde.Test.SampleTest.Address>.SerializeInstance
            => AddressSerializeProxy.Instance;

        sealed partial class AddressSerializeProxy :Serde.ISerialize<Serde.Test.SampleTest.Address>
        {
            void global::Serde.ISerialize<Serde.Test.SampleTest.Address>.Serialize(Serde.Test.SampleTest.Address value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<Address>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteString(_l_info, 0, value.Name);
                _l_type.WriteString(_l_info, 1, value.Line1);
                _l_type.WriteFieldIfNotNull<string?, Serde.NullableRefProxy.Serialize<string,global::Serde.StringProxy>>(_l_info, 2, value.City);
                _l_type.WriteString(_l_info, 3, value.State);
                _l_type.WriteString(_l_info, 4, value.Zip);
                _l_type.End(_l_info);
            }
            public static readonly AddressSerializeProxy Instance = new();
            private AddressSerializeProxy() { }

        }
    }
}
