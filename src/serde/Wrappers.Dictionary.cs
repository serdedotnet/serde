using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace Serde
{
    internal static class DictSerdeTypeInfo<TKey, TValue> where TKey : notnull
    {
        public static readonly TypeInfo TypeInfo = TypeInfo.Create(
            typeof(Dictionary<TKey, TValue>).Name, TypeInfo.TypeKind.Dictionary, []);
    }

    public static class DictWrap
    {
        public readonly struct SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap> : ISerialize<Dictionary<TKey, TValue>>
            where TKey : notnull
            where TKeyWrap : struct, ISerialize<TKey>
            where TValueWrap : struct, ISerialize<TValue>
        {
            public void Serialize(Dictionary<TKey, TValue> value, ISerializer serializer)
            {
                var typeInfo = DictSerdeTypeInfo<TKey, TValue>.TypeInfo;
                var sd = serializer.SerializeCollection(typeInfo, value.Count);
                foreach (var (k, v) in value)
                {
                    sd.SerializeElement(k, default(TKeyWrap));
                    sd.SerializeElement(v, default(TValueWrap));
                }
                sd.End(typeInfo);
            }
        }

        public readonly struct DeserializeImpl<TKey, TKeyWrap, TValue, TValueWrap> : IDeserialize<Dictionary<TKey, TValue>>
            where TKey : notnull
            where TKeyWrap : IDeserialize<TKey>
            where TValueWrap : IDeserialize<TValue>
        {
            public static Dictionary<TKey, TValue> Deserialize(IDeserializer deserializer)
            {
                var typeInfo = DictSerdeTypeInfo<TKey, TValue>.TypeInfo;
                var deCollection = deserializer.DeserializeCollection(typeInfo);
                Dictionary<TKey, TValue> dict;
                if (deCollection.SizeOpt is int size)
                {
                    dict = new(size);
                }
                else
                {
                    size = -1; // Set initial size to unknown
                    dict = new();
                }
                while (deCollection.TryReadValue<TKey, TKeyWrap>(typeInfo, out var key))
                {
                    if (!deCollection.TryReadValue<TValue, TValueWrap>(typeInfo, out var value))
                    {
                        throw new InvalidDeserializeValueException("Expected value, but reached end of collection.");
                    }
                    dict.Add(key, value);
                }
                if (size >= 0 && size != dict.Count)
                {
                    throw new InvalidDeserializeValueException($"Expected {size} items, found {dict.Count}");
                }
                return dict;
            }
        }
    }

    public static class IDictWrap
    {
        public readonly record struct SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap> : ISerialize<IDictionary<TKey, TValue>>
            where TKey : notnull
            where TKeyWrap : struct, ISerialize<TKey>
            where TValueWrap : struct, ISerialize<TValue>
        {
            public void Serialize(IDictionary<TKey, TValue> value, ISerializer serializer)
            {
                var typeInfo = DictSerdeTypeInfo<TKey, TValue>.TypeInfo;
                var sd = serializer.SerializeCollection(typeInfo, value.Count);
                foreach (var (k, v) in value)
                {
                    sd.SerializeElement(k, default(TKeyWrap));
                    sd.SerializeElement(v, default(TValueWrap));
                }
                sd.End(typeInfo);
            }
        }
    }

    public static class IRODictWrap
    {
        private static readonly TypeInfo s_typeInfo = TypeInfo.Create(
            typeof(IReadOnlyDictionary<,>).Name,
            TypeInfo.TypeKind.Dictionary,
            []);

        public readonly record struct SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap> : ISerialize<IReadOnlyDictionary<TKey, TValue>>
            where TKey : notnull
            where TKeyWrap : struct, ISerialize<TKey>
            where TValueWrap : struct, ISerialize<TValue>
        {
            public void Serialize(IReadOnlyDictionary<TKey, TValue> value, ISerializer serializer)
            {
                var typeInfo = s_typeInfo;
                var sd = serializer.SerializeCollection(typeInfo, value.Count);
                foreach (var (k, v) in value)
                {
                    sd.SerializeElement(k, default(TKeyWrap));
                    sd.SerializeElement(v, default(TValueWrap));
                }
                sd.End(typeInfo);
            }
        }
    }
}