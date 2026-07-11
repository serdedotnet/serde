
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record ContactGen
    {
        sealed partial class _SerObj : Serde.ISerialize<Serde.Test.JsonSerializerTests.ContactGen>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonSerializerTests.ContactGen.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.JsonSerializerTests.ContactGen>.Serialize(Serde.Test.JsonSerializerTests.ContactGen value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_fieldCount = 3;
                if (value.Name is null) _l_fieldCount--;
                if (value.Email is null) _l_fieldCount--;
                var _l_type = serializer.WriteType(_l_info, _l_fieldCount);
                _l_type.WriteI32(_l_info, 0, value.Id);
                _l_type.WriteStringIfNotNull(_l_info, 1, value.Name);
                _l_type.WriteStringIfNotNull(_l_info, 2, value.Email);
                _l_type.End(_l_info);
            }

        }
    }
}
