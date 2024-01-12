
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonSerializerTests
    {
        partial class NullableFields : Serde.ISerialize<Serde.Test.JsonSerializerTests.NullableFields>
        {
            void ISerialize<Serde.Test.JsonSerializerTests.NullableFields>.Serialize(Serde.Test.JsonSerializerTests.NullableFields value, ISerializer serializer)
            {
                var type = serializer.SerializeType("NullableFields", 2);
                type.SerializeFieldIfNotNull<string?, Serde.NullableRefWrap.SerializeImpl<string, StringWrap>>("s", value.S);
                type.SerializeField<System.Collections.Generic.Dictionary<string, string?>, Serde.DictWrap.SerializeImpl<string, StringWrap, string?, Serde.NullableRefWrap.SerializeImpl<string, StringWrap>>>("d", value.D);
                type.End();
            }
        }
    }
}