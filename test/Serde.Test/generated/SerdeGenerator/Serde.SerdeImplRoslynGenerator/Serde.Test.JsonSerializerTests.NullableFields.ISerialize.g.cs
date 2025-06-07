
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial class NullableFields
    {
        sealed partial class _SerObj : Serde.ISerialize<Serde.Test.JsonSerializerTests.NullableFields>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonSerializerTests.NullableFields.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.JsonSerializerTests.NullableFields>.Serialize(Serde.Test.JsonSerializerTests.NullableFields value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteStringIfNotNull(_l_info, 0, value.S);
                _l_type.WriteValue<System.Collections.Generic.Dictionary<string, string?>, Serde.DictProxy.Ser<string, string?, global::Serde.StringProxy, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>>(_l_info, 1, value.D);
                _l_type.End(_l_info);
            }

        }
    }
}
