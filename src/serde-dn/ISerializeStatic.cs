using System;
using System.Diagnostics;

namespace Serde
{
    public interface ISerializeStatic : ISerialize
    {
        void Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            where TSerializeType : ISerializeTypeStatic
            where TSerializeEnumerable : ISerializeEnumerableStatic
            where TSerializeDictionary : ISerializeDictionaryStatic
            where TSerializer : ISerializerStatic<TSerializeType, TSerializeEnumerable, TSerializeDictionary>;
    }

    public interface ISerializeTypeStatic : ISerializeType
    {
        new void SerializeField<T>(string name, T value) where T : ISerializeStatic;
    }

    public interface ISerializeEnumerableStatic : ISerializeEnumerable
    {
        new void SerializeElement<T>(T value) where T : ISerializeStatic;
    }

    public interface ISerializeDictionaryStatic : ISerializeDictionary
    {
        new void SerializeKey<T>(T key) where T : ISerializeStatic;
        new void SerializeValue<T>(T value) where T : ISerializeStatic;
    }

    public interface ISerializerStatic<
        out TSerializeType,
        out TSerializeEnumerable,
        out TSerializeDictionary
        > : ISerializer
        where TSerializeType : ISerializeTypeStatic
        where TSerializeEnumerable : ISerializeEnumerableStatic
        where TSerializeDictionary : ISerializeDictionaryStatic
    {
        new TSerializeType SerializeType(string name, int numFields);
        new TSerializeEnumerable SerializeEnumerable(int? length);
        new TSerializeDictionary SerializeDictionary(int? length);
    }
}
