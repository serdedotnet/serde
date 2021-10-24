// Contains implementations of data interfaces for core types

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Serde
{
    internal static class EnumerableHelpers
    {
        public static void SerializeArray<
            T,
            TWrap,
            TSerializer,
            TSerializeType,
            TSerializeEnumerable,
            TSerializeDictionary
            >(T[] arr, ref TSerializer serializer)
            where TWrap : struct, IWrap<T, TWrap>, ISerialize
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable, TSerializeDictionary>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
            where TSerializeDictionary : ISerializeDictionary
        {
            var enumerable = serializer.SerializeEnumerable(arr.Length);
            var wrapper = default(TWrap);
            foreach (var item in arr)
            {
                enumerable.SerializeElement(wrapper.Create(item));
            }
            enumerable.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeList<
            T,
            TWrap,
            TSerializer,
            TSerializeType,
            TSerializeEnumerable,
            TSerializeDictionary>(List<T> list, ref TSerializer serializer)
            where TWrap : struct, IWrap<T, TWrap>, ISerialize
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable, TSerializeDictionary>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
            where TSerializeDictionary : ISerializeDictionary
        {
            var enumerable = serializer.SerializeEnumerable(list.Count);
            var wrapper = default(TWrap);
            foreach (var item in list)
            {
                enumerable.SerializeElement(wrapper.Create(item));
            }
            enumerable.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeImmutableArray<
            T,
            TWrap,
            TSerializer,
            TSerializeType,
            TSerializeEnumerable,
            TSerializeDictionary>(ImmutableArray<T> list, ref TSerializer serializer)
            where TWrap : struct, IWrap<T, TWrap>, ISerialize
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable, TSerializeDictionary>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
            where TSerializeDictionary : ISerializeDictionary
        {
            var enumerable = serializer.SerializeEnumerable(list.Length);
            var wrapper = default(TWrap);
            foreach (var item in list)
            {
                enumerable.SerializeElement(wrapper.Create(item));
            }
            enumerable.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeIList<
            T,
            TWrap,
            TSerializer,
            TSerializeType,
            TSerializeEnumerable,
            TSerializeDictionary>(IList<T> list, ref TSerializer serializer)
            where TWrap : struct, IWrap<T, TWrap>, ISerialize
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable, TSerializeDictionary>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
            where TSerializeDictionary : ISerializeDictionary
        {
            var enumerable = serializer.SerializeEnumerable(list.Count);
            var wrapper = default(TWrap);
            foreach (var item in list)
            {
                enumerable.SerializeElement(wrapper.Create(item));
            }
            enumerable.End();
        }
    }

    public readonly struct IdWrap<T> : ISerialize, IWrap<T, IdWrap<T>>
        where T : ISerialize
    {
        public IdWrap<T> Create(T t) => new IdWrap<T>(t);
        private readonly T _t;
        public IdWrap(T t)
        {
            _t = t;
        }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            => _t.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref serializer);
    }

    public readonly struct ArrayWrap<T, TWrap> : ISerialize, IWrap<T[], ArrayWrap<T, TWrap>>
        where TWrap: struct, IWrap<T, TWrap>, ISerialize
    {
        public ArrayWrap<T, TWrap> Create(T[] t) => new ArrayWrap<T, TWrap>(t);

        private readonly T[] _a;
        public ArrayWrap(T[] a)
        {
            _a = a;
        }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            => EnumerableHelpers.SerializeArray<T, TWrap, TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(_a, ref serializer);
    }

    public readonly struct ArrayWrap<T> : ISerialize, IWrap<T[], ArrayWrap<T>>
        where T : ISerialize
    {
        public ArrayWrap<T> Create(T[] t) => new ArrayWrap<T>(t);

        private readonly T[] _a;
        public ArrayWrap(T[] a) { _a = a; }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            => EnumerableHelpers.SerializeArray<T, IdWrap<T>, TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(_a, ref serializer);
    }

    public readonly struct ListWrap<T> : ISerialize, IWrap<List<T>, ListWrap<T>>
        where T : ISerialize
    {
        public ListWrap<T> Create(List<T> t) => new ListWrap<T>(t);

        private readonly List<T> _a;
        public ListWrap(List<T> a) { _a = a; }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            => EnumerableHelpers.SerializeList<T, IdWrap<T>, TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(_a, ref serializer);
    }

    public readonly struct ListWrap<T, TWrap> : ISerialize, IWrap<List<T>, ListWrap<T, TWrap>>
        where TWrap : struct, IWrap<T, TWrap>, ISerialize
    {
        public ListWrap<T, TWrap> Create(List<T> t) => new ListWrap<T, TWrap>(t);

        private readonly List<T> _a;
        public ListWrap(List<T> a)
        {
            _a = a;
        }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            => EnumerableHelpers.SerializeList<T, TWrap, TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(_a, ref serializer);
    }

    public readonly struct ImmutableArrayWrap<T> : ISerialize, IWrap<ImmutableArray<T>, ImmutableArrayWrap<T>>
        where T : ISerialize
    {
        public ImmutableArrayWrap<T> Create(ImmutableArray<T> t) => new ImmutableArrayWrap<T>(t);

        private readonly ImmutableArray<T> _a;
        public ImmutableArrayWrap(ImmutableArray<T> a) { _a = a; }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            => EnumerableHelpers.SerializeImmutableArray<T, IdWrap<T>, TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(_a, ref serializer);
    }

    public readonly struct ImmutableArrayWrap<T, TWrap> : ISerialize, IWrap<ImmutableArray<T>, ImmutableArrayWrap<T, TWrap>>
        where TWrap : struct, IWrap<T, TWrap>, ISerialize
    {
        public ImmutableArrayWrap<T, TWrap> Create(ImmutableArray<T> t) => new ImmutableArrayWrap<T, TWrap>(t);

        private readonly ImmutableArray<T> _a;
        public ImmutableArrayWrap(ImmutableArray<T> a)
        {
            _a = a;
        }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            => EnumerableHelpers.SerializeImmutableArray<T, TWrap, TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(_a, ref serializer);
    }
}