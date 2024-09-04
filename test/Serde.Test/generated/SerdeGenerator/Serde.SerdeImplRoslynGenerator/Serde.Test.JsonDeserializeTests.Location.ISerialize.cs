
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial record Location : Serde.ISerialize<Serde.Test.JsonDeserializeTests.Location>
        {
            void ISerialize<Serde.Test.JsonDeserializeTests.Location>.Serialize(Serde.Test.JsonDeserializeTests.Location value, ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Location>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<int, global::Serde.Int32Wrap>(_l_serdeInfo, 0, value.Id);
                type.SerializeField<string, global::Serde.StringWrap>(_l_serdeInfo, 1, value.Address1);
                type.SerializeField<string, global::Serde.StringWrap>(_l_serdeInfo, 2, value.Address2);
                type.SerializeField<string, global::Serde.StringWrap>(_l_serdeInfo, 3, value.City);
                type.SerializeField<string, global::Serde.StringWrap>(_l_serdeInfo, 4, value.State);
                type.SerializeField<string, global::Serde.StringWrap>(_l_serdeInfo, 5, value.PostalCode);
                type.SerializeField<string, global::Serde.StringWrap>(_l_serdeInfo, 6, value.Name);
                type.SerializeField<string, global::Serde.StringWrap>(_l_serdeInfo, 7, value.PhoneNumber);
                type.SerializeField<string, global::Serde.StringWrap>(_l_serdeInfo, 8, value.Country);
                type.End();
            }
        }
    }
}