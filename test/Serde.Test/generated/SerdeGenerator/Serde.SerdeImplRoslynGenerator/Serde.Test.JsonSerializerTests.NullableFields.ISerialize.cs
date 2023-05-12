
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonSerializerTests
    {
        partial class NullableFields : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("NullableFields", 2);
                type.SerializeFieldIfNotNull("s"u8, new NullableRefWrap.SerializeImpl<string, StringWrap>(this.S), this.S);
                type.SerializeField("d"u8, new DictWrap.SerializeImpl<string, StringWrap, string?, NullableRefWrap.SerializeImpl<string, StringWrap>>(this.D));
                type.End();
            }
        }
    }
}