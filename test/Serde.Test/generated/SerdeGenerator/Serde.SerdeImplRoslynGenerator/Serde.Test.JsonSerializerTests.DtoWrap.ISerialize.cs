
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record DtoWrap
    {
        sealed partial class _SerObj : Serde.ISerialize<Serde.Test.JsonSerializerTests.DtoWrap>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonSerializerTests.DtoWrap.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.JsonSerializerTests.DtoWrap>.Serialize(Serde.Test.JsonSerializerTests.DtoWrap value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteBoxedValue<global::System.DateTimeOffset, global::Serde.DateTimeOffsetProxy>(_l_info, 0, value.Value);
                _l_type.End(_l_info);
            }

        }
    }
}
