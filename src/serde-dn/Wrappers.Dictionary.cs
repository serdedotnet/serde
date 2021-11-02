using System.Collections.Generic;

namespace Serde
{
    public static class DictWrap
    {
        public readonly struct SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap> : ISerialize,
            ISerializeWrap<Dictionary<TKey, TValue>, SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>>
            where TKey : notnull
            where TKeyWrap : struct, ISerializeWrap<TKey, TKeyWrap>, ISerialize
            where TValueWrap : struct, ISerializeWrap<TValue, TValueWrap>, ISerialize
        {
            public static SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap> Create(Dictionary<TKey, TValue> t)
                => new SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>(t);

            private readonly Dictionary<TKey, TValue> _dict;
            public SerializeImpl(Dictionary<TKey, TValue> dict)
            {
                _dict = dict;
            }

            void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            {
                var sd = serializer.SerializeDictionary(_dict.Count);
                foreach (var (k, v) in _dict)
                {
                    sd.SerializeKey(TKeyWrap.Create(k));
                    sd.SerializeValue(TValueWrap.Create(v));
                }
                sd.End();
            }
        }
    }

    public static class IDictWrap
    {
        public readonly record struct SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>(IDictionary<TKey, TValue> Value)
            : ISerialize, ISerializeWrap<IDictionary<TKey, TValue>, SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>>
            where TKey : notnull
            where TKeyWrap : struct, ISerializeWrap<TKey, TKeyWrap>, ISerialize
            where TValueWrap : struct, ISerializeWrap<TValue, TValueWrap>, ISerialize
        {
            public static SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap> Create(IDictionary<TKey, TValue> t)
                => new SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap>(t);

            void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
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

            void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
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