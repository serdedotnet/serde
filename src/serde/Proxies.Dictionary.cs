using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.Win32;

namespace Serde;

public abstract class SerDictBase<TSelf, TK, TV, TDict, TKProvider, TVProvider>
    : ISerialize<TDict>, ISerializeProvider<TDict>
    where TK : notnull
    where TSelf : ISerialize<TDict>, ISerdeInfoProvider, new()
    where TDict : IReadOnlyDictionary<TK, TV>
    where TKProvider : ISerializeProvider<TK>
    where TVProvider : ISerializeProvider<TV>
{
    public static TSelf Instance { get; } = new();
    static ISerialize<TDict> ISerializeProvider<TDict>.SerializeInstance => Instance;
    static ISerdeInfo ISerdeInfoProvider.SerdeInfo => TSelf.SerdeInfo;

    private readonly ITypeSerialize<TK> _keySer;
    private readonly ITypeSerialize<TV> _valueSer;

    protected SerDictBase()
    {
        var ks = TKProvider.SerializeInstance;
        _keySer = ks is ITypeSerialize<TK> ktSer
            ? ktSer
            : new TypeSerBoxed<TK>(ks);
        var vs = TVProvider.SerializeInstance;
        _valueSer = vs is ITypeSerialize<TV> kvSer
            ? kvSer
            : new TypeSerBoxed<TV>(vs);
    }

    void ISerialize<TDict>.Serialize(TDict value, ISerializer serializer)
    {
        var typeInfo = DictSerdeInfo<TK, TV>.Instance;
        var sd = serializer.WriteCollection(typeInfo, value.Count);
        int index = 0;
        foreach (var (k, v) in value)
        {
            _keySer.Serialize(k, sd, typeInfo, index++);
            _valueSer.Serialize(v, sd, typeInfo, index++);
        }
        sd.End(typeInfo);
    }
}

public abstract class DeDictBase<
        TSelf,
        TKey,
        TValue,
        TDict,
        TBuilder,
        TKProvider,
        TVProvider>
    : IDeserialize<TDict>,
      IDeserializeProvider<TDict>
    where TSelf : IDeserialize<TDict>, new()
    where TBuilder : ICollection
    where TKey : notnull
    where TKProvider : IDeserializeProvider<TKey>
    where TVProvider : IDeserializeProvider<TValue>
{
    public static IDeserialize<TDict> DeserializeInstance { get; } = new TSelf();
    public static ISerdeInfo SerdeInfo => DictSerdeInfo<TKey, TValue>.Instance;

    private readonly ITypeDeserialize<TKey> _keyDe;
    private readonly ITypeDeserialize<TValue> _valueDe;

    protected DeDictBase()
    {
        var keyDe = TKProvider.DeserializeInstance;
        var valueDe = TVProvider.DeserializeInstance;
        Debug.Assert(TKProvider.SerdeInfo.Kind != InfoKind.Primitive
            || keyDe is ITypeDeserialize<TKey>, $"{typeof(TKey)} does not implement ITypeDeserialize");
        Debug.Assert(TVProvider.SerdeInfo.Kind != InfoKind.Primitive
            || valueDe is ITypeDeserialize<TValue>, $"{typeof(TValue)} does not implement ITypeDeserialize");
        _keyDe = keyDe is ITypeDeserialize<TKey> keyTypeDe
            ? keyTypeDe
            : new TypeDeBoxed<TKey>(keyDe);
        _valueDe = valueDe is ITypeDeserialize<TValue> valueTypeDe
            ? valueTypeDe
            : new TypeDeBoxed<TValue>(valueDe);
    }

    public TDict Deserialize(IDeserializer deserializer)
    {
        var typeInfo = DictSerdeInfo<TKey, TValue>.Instance;
        var deCollection = deserializer.ReadCollection(typeInfo);
        var sizeOpt = deCollection.SizeOpt;
        TBuilder builder = GetBuilder(sizeOpt);
        int index;
        while ((index = deCollection.TryReadIndex(typeInfo, out _)) != ITypeDeserializer.EndOfType)
        {
            var key = _keyDe.Deserialize(deCollection, typeInfo, index);
            if ((index = deCollection.TryReadIndex(typeInfo, out _)) == ITypeDeserializer.EndOfType)
            {
                throw new DeserializeException("Expected value, but reached end of collection.");
            }
            var value = _valueDe.Deserialize(deCollection, typeInfo, index);
            Add(builder, key, value);
        }
        if (sizeOpt is int size && size != builder.Count)
        {
            throw new DeserializeException($"Expected {size} items, found {builder.Count}");
        }
        return Create(builder);
    }

    /// <summary>
    /// Creates the dictionary builder with the specified size.
    /// </summary>
    protected abstract TBuilder GetBuilder(int? sizeOpt);

    /// <summary>
    /// Creates the dictionary from the builder.
    /// </summary>
    protected abstract TDict Create(TBuilder builder);

    /// <summary>
    /// Adds a key-value pair to the dictionary builder.
    /// </summary>
    protected abstract void Add(TBuilder builder, TKey key, TValue value);
}

internal static class DictSerdeInfo<TKey, TValue> where TKey : notnull
{
    public static readonly ISerdeInfo Instance = SerdeInfo.MakeDictionary(typeof(Dictionary<TKey, TValue>).ToString());
}

public static class DictProxy
{
    public sealed class Ser<TKey, TValue, TKeyProvider, TValueProvider>
        : SerDictBase<
            Ser<TKey, TValue, TKeyProvider, TValueProvider>,
            TKey,
            TValue,
            Dictionary<TKey, TValue>,
            TKeyProvider,
            TValueProvider
          >,
          ISerializeProvider<Dictionary<TKey, TValue>>
        where TKey : notnull
        where TKeyProvider : ISerializeProvider<TKey>
        where TValueProvider : ISerializeProvider<TValue>
    {
        public static ISerdeInfo SerdeInfo => DictSerdeInfo<TKey, TValue>.Instance;
    }

    public sealed class De<TKey, TValue, TKProvider, TVProvider>
        : DeDictBase<
            De<TKey, TValue, TKProvider, TVProvider>,
            TKey,
            TValue,
            Dictionary<TKey, TValue>,
            Dictionary<TKey, TValue>,
            TKProvider,
            TVProvider
          >,
          IDeserializeProvider<Dictionary<TKey, TValue>>
        where TKey : notnull
        where TKProvider : IDeserializeProvider<TKey>
        where TVProvider : IDeserializeProvider<TValue>
    {
        protected override void Add(Dictionary<TKey, TValue> builder, TKey key, TValue value) => builder.Add(key, value);

        protected override Dictionary<TKey, TValue> Create(Dictionary<TKey, TValue> builder) => builder;

        protected override Dictionary<TKey, TValue> GetBuilder(int? sizeOpt)
        {
            if (sizeOpt is int size)
            {
                return new Dictionary<TKey, TValue>(size);
            }
            return new Dictionary<TKey, TValue>();
        }
    }
}