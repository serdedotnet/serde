using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Headers;

namespace Serde
{
    internal static class DictSerdeTypeInfo
    {
        public static readonly TypeInfo TypeInfo = TypeInfo.Create(TypeInfo.TypeKind.Dictionary, []);
    }

    public static class DictWrap
    {
        public readonly struct SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>
            : ISerialize, ISerialize<Dictionary<TKey, TValue>>,
              ISerializeWrap<Dictionary<TKey, TValue>, SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>>
            where TKey : notnull
            where TKeyWrap : struct, ISerializeWrap<TKey, TKeyWrap>, ISerialize, ISerialize<TKey>
            where TValueWrap : struct, ISerializeWrap<TValue, TValueWrap>, ISerialize, ISerialize<TValue>
        {
            public static SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap> Create(Dictionary<TKey, TValue> t)
                => new SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>(t);

            private readonly Dictionary<TKey, TValue> _dict;
            public SerializeImpl(Dictionary<TKey, TValue> dict)
            {
                _dict = dict;
            }

            void ISerialize.Serialize(ISerializer serializer)
            {
                var sd = serializer.SerializeDictionary(_dict.Count);
                foreach (var (k, v) in _dict)
                {
                    sd.SerializeKey(TKeyWrap.Create(k));
                    sd.SerializeValue(TValueWrap.Create(v));
                }
                sd.End();
            }

            void ISerialize<Dictionary<TKey, TValue>>.Serialize(Dictionary<TKey, TValue> value, ISerializer serializer)
            {
                var sd = serializer.SerializeDictionary(value.Count);
                foreach (var (k, v) in value)
                {
                    sd.SerializeKey(k, TKeyWrap.Create(k));
                    sd.SerializeValue(v, TValueWrap.Create(v));
                }
                sd.End();
            }
        }

        public readonly struct DeserializeImpl<TKey, TKeyWrap, TValue, TValueWrap> : IDeserialize<Dictionary<TKey, TValue>>
            where TKey : notnull
            where TKeyWrap : IDeserialize<TKey>
            where TValueWrap : IDeserialize<TValue>
        {
            static Dictionary<TKey, TValue> IDeserialize<Dictionary<TKey, TValue>>.Deserialize(IDeserializer deserializer)
            {
                var typeInfo = DictSerdeTypeInfo.TypeInfo;
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
        public readonly record struct SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>(IDictionary<TKey, TValue> Value)
            : ISerialize, ISerializeWrap<IDictionary<TKey, TValue>, SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>>,
              ISerialize<IDictionary<TKey, TValue>>
            where TKey : notnull
            where TKeyWrap : struct, ISerializeWrap<TKey, TKeyWrap>, ISerialize, ISerialize<TKey>
            where TValueWrap : struct, ISerializeWrap<TValue, TValueWrap>, ISerialize, ISerialize<TValue>
        {
            public static SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap> Create(IDictionary<TKey, TValue> t)
                => new SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>(t);

            void ISerialize<IDictionary<TKey, TValue>>.Serialize(IDictionary<TKey, TValue> value, ISerializer serializer)
            {
                var sd = serializer.SerializeDictionary(value.Count);
                foreach (var (k, v) in value)
                {
                    sd.SerializeKey(k, TKeyWrap.Create(k));
                    sd.SerializeValue(v, TValueWrap.Create(v));
                }
                sd.End();
            }
            void ISerialize.Serialize(ISerializer serializer)
            {
                var sd = serializer.SerializeDictionary(Value.Count);
                foreach (var (k, v) in Value)
                {
                    sd.SerializeKey(TKeyWrap.Create(k));
                    sd.SerializeValue(TValueWrap.Create(v));
                }
                sd.End();
            }
        }
    }

    public static class IRODictWrap
    {
        public readonly record struct SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>(IReadOnlyDictionary<TKey, TValue> Value)
            : ISerialize,
            ISerializeWrap<IReadOnlyDictionary<TKey, TValue>, SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>>
            where TKey : notnull
            where TKeyWrap : struct, ISerializeWrap<TKey, TKeyWrap>, ISerialize
            where TValueWrap : struct, ISerializeWrap<TValue, TValueWrap>, ISerialize
        {
            public static SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap> Create(IReadOnlyDictionary<TKey, TValue> t)
                => new SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>(t);

            void ISerialize.Serialize(ISerializer serializer)
            {
                var sd = serializer.SerializeDictionary(Value.Count);
                foreach (var (k, v) in Value)
                {
                    sd.SerializeKey(TKeyWrap.Create(k));
                    sd.SerializeValue(TValueWrap.Create(v));
                }
                sd.End();
            }
        }
    }
}