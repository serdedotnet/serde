using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.Win32;

namespace Serde;

public abstract class SerDictBase<TSelf, TK, TV, TDict, TKProvider, TVProvider>
    : ISerialize<TDict>, ISerializeProvider<TDict>
    where TK : notnull
    where TSelf : ISerialize<TDict>, new()
    where TDict : IReadOnlyDictionary<TK, TV>
    where TKProvider : ISerializeProvider<TK>
    where TVProvider : ISerializeProvider<TV>
{
    public static TSelf Instance { get; } = new();
    static ISerialize<TDict> ISerializeProvider<TDict>.Instance => Instance;
    public abstract ISerdeInfo SerdeInfo { get; }

    private readonly ITypeSerialize<TK> _keySer;
    private readonly ITypeSerialize<TV> _valueSer;

    protected SerDictBase()
    {
        _keySer = TypeSerialize.GetOrBox<TK, TKProvider>();
        _valueSer = TypeSerialize.GetOrBox<TV, TVProvider>();
    }

    void ISerialize<TDict>.Serialize(TDict value, ISerializer serializer)
    {
        var typeInfo = SerdeInfo;
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
    public static IDeserialize<TDict> Instance { get; } = new TSelf();
    public abstract ISerdeInfo SerdeInfo { get; }

    private readonly ITypeDeserialize<TKey> _keyDe = TypeDeserialize.GetOrBox<TKey, TKProvider>();
    private readonly ITypeDeserialize<TValue> _valueDe = TypeDeserialize.GetOrBox<TValue, TVProvider>();

    protected DeDictBase() { }

    public TDict Deserialize(IDeserializer deserializer)
    {
        var typeInfo = SerdeInfo;
        var deCollection = deserializer.ReadType(typeInfo);
        var sizeOpt = deCollection.SizeOpt;
        TBuilder builder = GetBuilder(sizeOpt);
        int index;
        while (true)
        {
            index = deCollection.TryReadIndex(typeInfo);
            if (index == ITypeDeserializer.EndOfType)
            {
                break;
            }

            var key = _keyDe.Deserialize(deCollection, typeInfo, index);
            index = deCollection.TryReadIndex(typeInfo);
            if (index == ITypeDeserializer.EndOfType)
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
        deCollection.End(typeInfo);
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

public static class DictProxy
{
    public static ISerdeInfo GetSerdeInfo<TKey, TValue>(ISerdeInfo keyInfo, ISerdeInfo valueInfo)
        where TKey : notnull
        => SerdeInfo.MakeDictionary(typeof(Dictionary<TKey, TValue>).ToString(), keyInfo, valueInfo);

    public sealed class Ser<TKey, TValue, TKeyProvider, TValueProvider>()
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
        private static readonly ISerdeInfo s_serdeInfo = GetSerdeInfo<TKey, TValue>(
            TKeyProvider.Instance.SerdeInfo,
            TValueProvider.Instance.SerdeInfo);
        public override ISerdeInfo SerdeInfo => s_serdeInfo;
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
        private static readonly ISerdeInfo s_serdeInfo = GetSerdeInfo<TKey, TValue>(
            TKProvider.Instance.SerdeInfo,
            TVProvider.Instance.SerdeInfo);
        public override ISerdeInfo SerdeInfo => s_serdeInfo;

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