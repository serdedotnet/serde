using System.Collections.Generic;
using System.Collections.Immutable;

namespace Serde;

internal static class DictSerdeInfo<TKey, TValue> where TKey : notnull
{
    public static readonly ISerdeInfo Instance = SerdeInfo.MakeDictionary(typeof(Dictionary<TKey, TValue>).ToString());
}

public static class DictWrap
{
    public readonly struct SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap> : ISerialize<Dictionary<TKey, TValue>>
        where TKey : notnull
        where TKeyWrap : struct, ISerialize<TKey>
        where TValueWrap : struct, ISerialize<TValue>
    {
        public static ISerdeInfo SerdeInfo => DictSerdeInfo<TKey, TValue>.Instance;
        public void Serialize(Dictionary<TKey, TValue> value, ISerializer serializer)
        {
            var typeInfo = DictSerdeInfo<TKey, TValue>.Instance;
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
        public static ISerdeInfo SerdeInfo => DictSerdeInfo<TKey, TValue>.Instance;
        public static Dictionary<TKey, TValue> Deserialize(IDeserializer deserializer)
        {
            var typeInfo = DictSerdeInfo<TKey, TValue>.Instance;
            var deCollection = deserializer.ReadCollection(typeInfo);
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
                    throw new DeserializeException("Expected value, but reached end of collection.");
                }
                dict.Add(key, value);
            }
            if (size >= 0 && size != dict.Count)
            {
                throw new DeserializeException($"Expected {size} items, found {dict.Count}");
            }
            return dict;
        }
    }
}

public static class ImmutableDictWrap
{
    public readonly struct SerializeImpl<TKey, TKeyWrap, TValue, TValueWrap> : ISerialize<ImmutableDictionary<TKey, TValue>>
        where TKey : notnull
        where TKeyWrap : struct, ISerialize<TKey>
        where TValueWrap : struct, ISerialize<TValue>
    {
        public static ISerdeInfo SerdeInfo => DictSerdeInfo<TKey, TValue>.Instance;
        public void Serialize(ImmutableDictionary<TKey, TValue> value, ISerializer serializer)
        {
            var typeInfo = DictSerdeInfo<TKey, TValue>.Instance;
            var sd = serializer.SerializeCollection(typeInfo, value.Count);
            foreach (var (k, v) in value)
            {
                sd.SerializeElement(k, default(TKeyWrap));
                sd.SerializeElement(v, default(TValueWrap));
            }
            sd.End(typeInfo);
        }
    }

    public readonly struct DeserializeImpl<TKey, TKeyWrap, TValue, TValueWrap> : IDeserialize<ImmutableDictionary<TKey, TValue>>
        where TKey : notnull
        where TKeyWrap : IDeserialize<TKey>
        where TValueWrap : IDeserialize<TValue>
    {
        public static ISerdeInfo SerdeInfo => DictSerdeInfo<TKey, TValue>.Instance;
        public static ImmutableDictionary<TKey, TValue> Deserialize(IDeserializer deserializer)
        {
            var typeInfo = DictSerdeInfo<TKey, TValue>.Instance;
            var deCollection = deserializer.ReadCollection(typeInfo);
            var builder = ImmutableDictionary.CreateBuilder<TKey, TValue>();
            while (deCollection.TryReadValue<TKey, TKeyWrap>(typeInfo, out var key))
            {
                if (!deCollection.TryReadValue<TValue, TValueWrap>(typeInfo, out var value))
                {
                    throw new DeserializeException("Expected value, but reached end of collection.");
                }
                builder.Add(key, value);
            }
            if (deCollection.SizeOpt is int size && size != builder.Count)
            {
                throw new DeserializeException($"Expected {size} items, found {builder.Count}");
            }
            return builder.ToImmutable();
        }
    }
}