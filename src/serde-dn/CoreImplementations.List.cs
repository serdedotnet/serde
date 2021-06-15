// Contains implementations of data interfaces for core types

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Serde
{
    internal static class EnumerableHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeArray<T, TWrapper, TWrapped, TSerializer, TSerializeType, TSerializeEnumerable>(T[] arr, ref TSerializer serializer)
            where TWrapped : ISerialize
            where TWrapper : IWrap<T, TWrapped>
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
        {
            var enumerable = serializer.SerializeEnumerable(arr.Length);
            var wrapper = default(TWrapper);
            foreach (var item in arr)
            {
                enumerable.SerializeElement(wrapper.Create(item));
            }
            enumerable.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeList<T, TWrapper, TWrapped, TSerializer, TSerializeType, TSerializeEnumerable>(List<T> list, ref TSerializer serializer)
            where TWrapped : ISerialize
            where TWrapper : IWrap<T, TWrapped>
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
        {
            var enumerable = serializer.SerializeEnumerable(list.Count);
            var wrapper = default(TWrapper);
            foreach (var item in list)
            {
                enumerable.SerializeElement(wrapper.Create(item));
            }
            enumerable.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeImmutableArray<T, TWrapper, TWrapped, TSerializer, TSerializeType, TSerializeEnumerable>(ImmutableArray<T> list, ref TSerializer serializer)
            where TWrapped : ISerialize
            where TWrapper : IWrap<T, TWrapped>
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
        {
            var enumerable = serializer.SerializeEnumerable(list.Length);
            var wrapper = default(TWrapper);
            foreach (var item in list)
            {
                enumerable.SerializeElement(wrapper.Create(item));
            }
            enumerable.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeIList<T, TList, TWrapper, TWrapped, TSerializer, TSerializeType, TSerializeEnumerable>(TList list, ref TSerializer serializer)
            where TList : IList<T>
            where TWrapped : ISerialize
            where TWrapper : IWrap<T, TWrapped>
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
        {
            var enumerable = serializer.SerializeEnumerable(list.Count);
            var wrapper = default(TWrapper);
            foreach (var item in list)
            {
                enumerable.SerializeElement(wrapper.Create(item));
            }
            enumerable.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeEnumerable<T, TEnum, TWrapper, TWrapped, TSerializer, TSerializeType, TSerializeEnumerable>(TEnum e, ref TSerializer serializer)
            where TEnum : IEnumerable<T>
            where TWrapped : ISerialize
            where TWrapper : IWrap<T, TWrapped>
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
        {
            var enumerable = serializer.SerializeEnumerable(null);
            var wrapper = default(TWrapper);
            foreach (var item in e)
            {
                enumerable.SerializeElement(wrapper.Create(item));
            }
            enumerable.End();
        }
    }

    public readonly struct IdWrap<T> : IWrap<T, T>
        where T : ISerialize
    {
        public T Create(T t) => t;
    }

    public readonly struct ArrayWrap<T, TWrap> : ISerialize, IWrap<T[], ArrayWrap<T, TWrap>>
        where TWrap: ISerialize, IWrap<T, TWrap>
    {
        public ArrayWrap<T, TWrap> Create(T[] t) => new ArrayWrap<T, TWrap>(t);

        private readonly T[] _a;
        public ArrayWrap(T[] a)
        {
            _a = a;
        }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
            => EnumerableHelpers.SerializeArray<
                T,
                TWrap,
                TWrap,
                TSerializer,
                TSerializeType,
                TSerializeEnumerable>(_a, ref serializer);
    }

    public readonly struct ArrayWrap<T> : ISerialize, IWrap<T[], ArrayWrap<T>>
        where T : ISerialize
    {
        public ArrayWrap<T> Create(T[] t) => new ArrayWrap<T>(t);

        private readonly T[] _a;
        public ArrayWrap(T[] a) { _a = a; }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
            => EnumerableHelpers.SerializeArray<
                T,
                IdWrap<T>,
                T,
                TSerializer,
                TSerializeType,
                TSerializeEnumerable>(_a, ref serializer);
    }

    public readonly struct ListWrap<T> : ISerialize, IWrap<List<T>, ListWrap<T>>
        where T : ISerialize
    {
        public ListWrap<T> Create(List<T> t) => new ListWrap<T>(t);

        private readonly List<T> _a;
        public ListWrap(List<T> a) { _a = a; }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
            => EnumerableHelpers.SerializeList<
                T,
                IdWrap<T>,
                T,
                TSerializer,
                TSerializeType,
                TSerializeEnumerable>(_a, ref serializer);
    }

    public readonly struct ListWrap<T, TWrap> : ISerialize, IWrap<List<T>, ListWrap<T, TWrap>>
        where TWrap : ISerialize, IWrap<T, TWrap>
    {
        public ListWrap<T, TWrap> Create(List<T> t) => new ListWrap<T, TWrap>(t);

        private readonly List<T> _a;
        public ListWrap(List<T> a)
        {
            _a = a;
        }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
            => EnumerableHelpers.SerializeList<
                T,
                TWrap,
                TWrap,
                TSerializer,
                TSerializeType,
                TSerializeEnumerable>(_a, ref serializer);
    }

    public readonly struct ImmutableArrayWrap<T> : ISerialize, IWrap<ImmutableArray<T>, ImmutableArrayWrap<T>>
        where T : ISerialize
    {
        public ImmutableArrayWrap<T> Create(ImmutableArray<T> t) => new ImmutableArrayWrap<T>(t);

        private readonly ImmutableArray<T> _a;
        public ImmutableArrayWrap(ImmutableArray<T> a) { _a = a; }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
            => EnumerableHelpers.SerializeImmutableArray<
                T,
                IdWrap<T>,
                T,
                TSerializer,
                TSerializeType,
                TSerializeEnumerable>(_a, ref serializer);
    }

    public readonly struct ImmutableArrayWrap<T, TWrap> : ISerialize, IWrap<ImmutableArray<T>, ImmutableArrayWrap<T, TWrap>>
        where TWrap : ISerialize, IWrap<T, TWrap>
    {
        public ImmutableArrayWrap<T, TWrap> Create(ImmutableArray<T> t) => new ImmutableArrayWrap<T, TWrap>(t);

        private readonly ImmutableArray<T> _a;
        public ImmutableArrayWrap(ImmutableArray<T> a)
        {
            _a = a;
        }

        void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
            => EnumerableHelpers.SerializeImmutableArray<
                T,
                TWrap,
                TWrap,
                TSerializer,
                TSerializeType,
                TSerializeEnumerable>(_a, ref serializer);
    }
}