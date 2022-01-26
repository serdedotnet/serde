
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonSerializerTests
    {
        partial class NullableFields : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            {
                var type = serializer.SerializeType("NullableFields", 2);
                type.SerializeField("S", new NullableRefWrap<string, StringWrap>(this.S));
                type.SerializeField("D", new DictWrap.SerializeImpl<string, StringWrap, string?, NullableRefWrap<string, StringWrap>>(this.D));
                type.End();
            }
        }
    }
}