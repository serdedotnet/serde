
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial struct Color
    {
        sealed partial class _SerObj : Serde.ISerialize<Serde.Test.JsonSerializerTests.Color>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonSerializerTests.Color.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.JsonSerializerTests.Color>.Serialize(Serde.Test.JsonSerializerTests.Color value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteI32(_l_info, 0, value.Red);
                _l_type.WriteI32(_l_info, 1, value.Green);
                _l_type.WriteI32(_l_info, 2, value.Blue);
                _l_type.End(_l_info);
            }

        }
    }
}
