
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonSerializerTests
    {
        partial struct Color : Serde.ISerialize<Serde.Test.JsonSerializerTests.Color>
        {
            void ISerialize<Serde.Test.JsonSerializerTests.Color>.Serialize(Serde.Test.JsonSerializerTests.Color value, ISerializer serializer)
            {
                var _l_serdeInfo = ColorSerdeInfo.Instance;
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<int, Int32Wrap>(_l_serdeInfo, 0, value.Red);
                type.SerializeField<int, Int32Wrap>(_l_serdeInfo, 1, value.Green);
                type.SerializeField<int, Int32Wrap>(_l_serdeInfo, 2, value.Blue);
                type.End();
            }
        }
    }
}