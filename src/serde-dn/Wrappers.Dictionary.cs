using System.Collections.Generic;

namespace Serde
{
    public readonly struct DictWrap<TKey, TKeyWrap, TValue, TValueWrap> : ISerialize,
        IWrap<Dictionary<TKey, TValue>, DictWrap<TKey, TKeyWrap, TValue, TValueWrap>>
        where TKey : notnull
        where TKeyWrap : struct, IWrap<TKey, TKeyWrap>, ISerialize
        where TValueWrap : struct, IWrap<TValue, TValueWrap>, ISerialize
    {
        public DictWrap<TKey, TKeyWrap, TValue, TValueWrap> Create(Dictionary<TKey, TValue> t)
            => new DictWrap<TKey, TKeyWrap, TValue, TValueWrap>(t);

        private readonly Dictionary<TKey, TValue> _dict;
        public DictWrap(Dictionary<TKey, TValue> dict)
        {
            _dict = dict;
        }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
        {
            var sd = serializer.SerializeDictionary(_dict.Count);
            var kwrap = default(TKeyWrap);
            var vwrap = default(TValueWrap);
            foreach (var (k, v) in _dict)
            {
                sd.SerializeKey(kwrap.Create(k));
                sd.SerializeValue(vwrap.Create(v));
            }
            sd.End();
        }
    }

    public readonly struct IDictWrap<TKey, TKeyWrap, TValue, TValueWrap> : ISerialize,
        IWrap<IDictionary<TKey, TValue>, IDictWrap<TKey, TKeyWrap, TValue, TValueWrap>>
        where TKey : notnull
        where TKeyWrap : struct, IWrap<TKey, TKeyWrap>, ISerialize
        where TValueWrap : struct, IWrap<TValue, TValueWrap>, ISerialize
    {
        public IDictWrap<TKey, TKeyWrap, TValue, TValueWrap> Create(IDictionary<TKey, TValue> t)
            => new IDictWrap<TKey, TKeyWrap, TValue, TValueWrap>(t);
        private readonly IDictionary<TKey, TValue> _dict;
        public IDictWrap(IDictionary<TKey, TValue> dict)
        {
            _dict = dict;
        }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
        {
            var sd = serializer.SerializeDictionary(_dict.Count);
            var kwrap = default(TKeyWrap);
            var vwrap = default(TValueWrap);
            foreach (var (k, v) in _dict)
            {
                sd.SerializeKey(kwrap.Create(k));
                sd.SerializeValue(vwrap.Create(v));
            }
            sd.End();
        }
    }

    public readonly struct IRODictWrap<TKey, TKeyWrap, TValue, TValueWrap> : ISerialize,
        IWrap<IReadOnlyDictionary<TKey, TValue>, IRODictWrap<TKey, TKeyWrap, TValue, TValueWrap>>
        where TKey : notnull
        where TKeyWrap : struct, IWrap<TKey, TKeyWrap>, ISerialize
        where TValueWrap : struct, IWrap<TValue, TValueWrap>, ISerialize
    {
        public IRODictWrap<TKey, TKeyWrap, TValue, TValueWrap> Create(IReadOnlyDictionary<TKey, TValue> t)
            => new IRODictWrap<TKey, TKeyWrap, TValue, TValueWrap>(t);
        private readonly IReadOnlyDictionary<TKey, TValue> _dict;
        public IRODictWrap(IReadOnlyDictionary<TKey, TValue> dict)
        {
            _dict = dict;
        }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
        {
            var sd = serializer.SerializeDictionary(_dict.Count);
            var kwrap = default(TKeyWrap);
            var vwrap = default(TValueWrap);
            foreach (var (k, v) in _dict)
            {
                sd.SerializeKey(kwrap.Create(k));
                sd.SerializeValue(vwrap.Create(v));
            }
            sd.End();
        }
    }
}