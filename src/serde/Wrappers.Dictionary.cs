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
        }

        public readonly struct DeserializeImpl<TKey, TKeyWrap, TValue, TValueWrap> : IDeserialize<Dictionary<TKey, TValue>>
            where TKey : notnull
            where TKeyWrap : IDeserialize<TKey>
            where TValueWrap : IDeserialize<TValue>
        {
            static Dictionary<TKey, TValue> IDeserialize<Dictionary<TKey, TValue>>.Deserialize<D>(ref D deserializer)
            {
                return deserializer.DeserializeDictionary<Dictionary<TKey, TValue>, Visitor>(new Visitor());
            }
            private struct Visitor : IDeserializeVisitor<Dictionary<TKey, TValue>>
            {
                public string ExpectedTypeName => "Dictionary<" + typeof(TKey).Name + ", " + typeof(TValue).Name + ">";
                public Dictionary<TKey, TValue> VisitDictionary<D>(ref D d)
                    where D : IDeserializeDictionary
                {
                    Dictionary<TKey, TValue> dict;
                    if (d.SizeOpt is int size)
                    {
                        dict = new Dictionary<TKey, TValue>(size);
                    }
                    else
                    {
                        size = -1; // Set initial size to unknown
                        dict = new Dictionary<TKey, TValue>();
                    }
                    while (d.TryGetNextEntry<TKey, TKeyWrap, TValue, TValueWrap>(out var next))
                    {
                        dict.Add(next.Item1, next.Item2);
                    }
                    if (size >= 0 && size != dict.Count)
                    {
                        throw new InvalidDeserializeValueException($"Expected {size} items, found {dict.Count}");
                    }
                    return dict;
                }
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