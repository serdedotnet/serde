using System.Collections.Generic;
using System.Collections.Immutable;

namespace Serde;

internal static class DictSerdeInfo<TKey, TValue> where TKey : notnull
{
    public static readonly ISerdeInfo Instance = SerdeInfo.MakeDictionary(typeof(Dictionary<TKey, TValue>).ToString());
}

public static class DictProxy
{
    public sealed class Serialize<TKey, TValue, TKeyProvider, TValueProvider>
        : SerializeImpl<TKey, TValue, ISerialize<TKey>, ISerialize<TValue>>,
          ISerializeProvider<Dictionary<TKey, TValue>>
        where TKey : notnull
        where TKeyProvider : ISerializeProvider<TKey>
        where TValueProvider : ISerializeProvider<TValue>
    {
        public static Serialize<TKey, TValue, TKeyProvider, TValueProvider> Instance { get; } = new();
        static ISerialize<Dictionary<TKey, TValue>> ISerializeProvider<Dictionary<TKey, TValue>>.SerializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => DictSerdeInfo<TKey, TValue>.Instance;
        private Serialize() : base(TKeyProvider.SerializeInstance, TValueProvider.SerializeInstance) { }
    }

    public class SerializeImpl<TKey, TValue, TKeyProxy, TValueProxy>(TKeyProxy kp, TValueProxy vp) : ISerialize<Dictionary<TKey, TValue>>
        where TKey : notnull
        where TKeyProxy : ISerialize<TKey>
        where TValueProxy : ISerialize<TValue>
    {
        public void Serialize(Dictionary<TKey, TValue> value, ISerializer serializer)
        {
            var typeInfo = DictSerdeInfo<TKey, TValue>.Instance;
            var sd = serializer.WriteCollection(typeInfo, value.Count);
            foreach (var (k, v) in value)
            {
                sd.WriteElement(k, kp);
                sd.WriteElement(v, vp);
            }
            sd.End(typeInfo);
        }
    }

    public sealed class Deserialize<TKey, TValue, TKeyProvider, TValueProvider>
        : DeserializeInstance<TKey, TValue, IDeserialize<TKey>, IDeserialize<TValue>>,
          IDeserializeProvider<Dictionary<TKey, TValue>>
        where TKey : notnull
        where TKeyProvider : IDeserializeProvider<TKey>
        where TValueProvider : IDeserializeProvider<TValue>
    {
        public static Deserialize<TKey, TValue, TKeyProvider, TValueProvider> Instance { get; } = new();
        static IDeserialize<Dictionary<TKey, TValue>> IDeserializeProvider<Dictionary<TKey, TValue>>.DeserializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => DictSerdeInfo<TKey, TValue>.Instance;
        private Deserialize() : base(TKeyProvider.DeserializeInstance, TValueProvider.DeserializeInstance) { }
    }

    public class DeserializeInstance<TKey, TValue, TKProxy, TVProxy>(TKProxy keyProxy, TVProxy valueProxy) : IDeserialize<Dictionary<TKey, TValue>>
        where TKey : notnull
        where TKProxy : IDeserialize<TKey>
        where TVProxy : IDeserialize<TValue>
    {
        public Dictionary<TKey, TValue> Deserialize(IDeserializer deserializer)
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
            while (deCollection.TryReadValue<TKey, TKProxy>(typeInfo, keyProxy, out var key))
            {
                if (!deCollection.TryReadValue<TValue, TVProxy>(typeInfo, valueProxy, out var value))
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
    public sealed class Serialize<TKey, TValue, TKeyProvider, TValueProvider>
        : SerializeInstance<TKey, TValue, ISerialize<TKey>, ISerialize<TValue>>,
          ISerializeProvider<ImmutableDictionary<TKey, TValue>>
        where TKey : notnull
        where TKeyProvider : ISerializeProvider<TKey>
        where TValueProvider : ISerializeProvider<TValue>
    {
        public static Serialize<TKey, TValue, TKeyProvider, TValueProvider> Instance { get; } = new();
        static ISerialize<ImmutableDictionary<TKey, TValue>> ISerializeProvider<ImmutableDictionary<TKey, TValue>>.SerializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => DictSerdeInfo<TKey, TValue>.Instance;
        private Serialize() : base(TKeyProvider.SerializeInstance, TValueProvider.SerializeInstance) { }
    }

    public class SerializeInstance<TKey, TValue, TKeyProxy, TValueProxy>(TKeyProxy keyProxy, TValueProxy valueProxy)
        : ISerialize<ImmutableDictionary<TKey, TValue>>
        where TKey : notnull
        where TKeyProxy : ISerialize<TKey>
        where TValueProxy : ISerialize<TValue>
    {
        public void Serialize(ImmutableDictionary<TKey, TValue> value, ISerializer serializer)
        {
            var typeInfo = DictSerdeInfo<TKey, TValue>.Instance;
            var sd = serializer.WriteCollection(typeInfo, value.Count);
            foreach (var (k, v) in value)
            {
                sd.WriteElement(k, keyProxy);
                sd.WriteElement(v, valueProxy);
            }
            sd.End(typeInfo);
        }
    }

    public sealed class Deserialize<TKey, TValue, TKeyProvider, TValueProvider>
        : DeserializeInstance<TKey, TValue, IDeserialize<TKey>, IDeserialize<TValue>>,
          IDeserializeProvider<ImmutableDictionary<TKey, TValue>>
        where TKey : notnull
        where TKeyProvider : IDeserializeProvider<TKey>
        where TValueProvider : IDeserializeProvider<TValue>
    {
        public static Deserialize<TKey, TValue, TKeyProvider, TValueProvider> Instance { get; } = new();
        static IDeserialize<ImmutableDictionary<TKey, TValue>> IDeserializeProvider<ImmutableDictionary<TKey, TValue>>.DeserializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => DictSerdeInfo<TKey, TValue>.Instance;
        private Deserialize() : base(TKeyProvider.DeserializeInstance, TValueProvider.DeserializeInstance) { }
    }

    public class DeserializeInstance<TKey, TValue, TKeyProxy, TValueProxy>(TKeyProxy keyProxy, TValueProxy valueProxy)
        : IDeserialize<ImmutableDictionary<TKey, TValue>>
        where TKey : notnull
        where TKeyProxy : IDeserialize<TKey>
        where TValueProxy : IDeserialize<TValue>
    {
        public ImmutableDictionary<TKey, TValue> Deserialize(IDeserializer deserializer)
        {
            var typeInfo = DictSerdeInfo<TKey, TValue>.Instance;
            var deCollection = deserializer.ReadCollection(typeInfo);
            var builder = ImmutableDictionary.CreateBuilder<TKey, TValue>();
            while (deCollection.TryReadValue<TKey, TKeyProxy>(typeInfo, keyProxy, out var key))
            {
                if (!deCollection.TryReadValue<TValue, TValueProxy>(typeInfo, valueProxy, out var value))
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