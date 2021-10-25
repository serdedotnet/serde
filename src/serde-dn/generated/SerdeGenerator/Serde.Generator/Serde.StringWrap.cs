
using Serde;

namespace Serde
{
    partial struct StringWrap : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
        {
            serializer.SerializeString(_s);
        }
    }
}