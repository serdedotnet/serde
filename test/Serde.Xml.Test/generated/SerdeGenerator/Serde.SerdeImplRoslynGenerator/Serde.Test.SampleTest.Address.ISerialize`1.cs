
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class SampleTest
    {
        partial record Address : Serde.ISerialize<Serde.Test.SampleTest.Address>
        {
            void ISerialize<Serde.Test.SampleTest.Address>.Serialize(Serde.Test.SampleTest.Address value, ISerializer serializer)
            {
                var _l_serdeInfo = AddressSerdeTypeInfo.TypeInfo;
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<string, StringWrap>(_l_serdeInfo, 0, value.Name);
                type.SerializeField<string, StringWrap>(_l_serdeInfo, 1, value.Line1);
                type.SerializeFieldIfNotNull<string?, Serde.NullableRefWrap.SerializeImpl<string, StringWrap>>(_l_serdeInfo, 2, value.City);
                type.SerializeField<string, StringWrap>(_l_serdeInfo, 3, value.State);
                type.SerializeField<string, StringWrap>(_l_serdeInfo, 4, value.Zip);
                type.End();
            }
        }
    }
}