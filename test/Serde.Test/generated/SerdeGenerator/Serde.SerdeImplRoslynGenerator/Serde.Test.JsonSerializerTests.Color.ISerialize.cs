
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
                var _l_typeInfo = ColorSerdeTypeInfo.TypeInfo;
                var type = serializer.SerializeType(_l_typeInfo);
                type.SerializeField<int, Int32Wrap>(_l_typeInfo, 0, this.Red);
                type.SerializeField<int, Int32Wrap>(_l_typeInfo, 1, this.Green);
                type.SerializeField<int, Int32Wrap>(_l_typeInfo, 2, this.Blue);
                type.End();
            }
        }
    }
}