// Contains implementations of data interfaces for core types

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Serde
{
    public static class EnumerableHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeSpan<T, U>(ISerdeInfo typeInfo, ReadOnlySpan<T> arr, U serializeImpl, ISerializer serializer)
            where U : ISerialize<T>
        {
            var enumerable = serializer.SerializeCollection(typeInfo, arr.Length);
            foreach (var item in arr)
            {
                enumerable.SerializeElement(item, serializeImpl);
            }
            enumerable.End(typeInfo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeSpan<T, U>(ISerdeInfo typeInfo, ReadOnlySpan<T> arr, ISerializer serializer)
            where U : struct, ISerialize<T>
            => SerializeSpan(typeInfo, arr, default(U), serializer);
    }

    internal static class ArraySerdeTypeInfo<T>
    {
        public static readonly ISerdeInfo TypeInfo = SerdeInfo.MakeEnumerable(typeof(T[]).ToString());
    }

    public static class ArrayWrap
    {
        public readonly struct SerializeImpl<T, TWrap> : ISerialize<T[]>
           where TWrap : struct, ISerialize<T>
        {
            public static ISerdeInfo SerdeInfo => ArraySerdeTypeInfo<T>.TypeInfo;
            public void Serialize(T[] value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(ArraySerdeTypeInfo<T>.TypeInfo, value, serializer);
        }

        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<T[]>
            where TWrap : IDeserialize<T>
        {
            public static ISerdeInfo SerdeInfo => ArraySerdeTypeInfo<T>.TypeInfo;
            public static T[] Deserialize(IDeserializer deserializer)
            {
                var typeInfo = ArraySerdeTypeInfo<T>.TypeInfo;
                var deCollection = deserializer.ReadCollection(typeInfo);
                if (deCollection.SizeOpt is int size)
                {
                    var array = new T[size];
                    for (int i = 0; i < size; i++)
                    {
                        if (!deCollection.TryReadValue<T, TWrap>(typeInfo, out var value))
                        {
                            throw new DeserializeException($"Expected array of size {size}, but only received {i} items");
                        }
                        array[i] = value;
                    }
                    return array;
                }
                else
                {
                    var list = new List<T>();
                    while (deCollection.TryReadValue<T, TWrap>(typeInfo, out var value))
                    {
                        list.Add(value);
                    }
                    return list.ToArray();
                }
            }
        }
    }

    internal static class ListSerdeTypeInfo<T>
    {
        public static readonly ISerdeInfo TypeInfo = SerdeInfo.MakeEnumerable(typeof(List<T>).ToString());
    }

    public static class ListWrap
    {
        public readonly struct SerializeImpl<T, TWrap> : ISerialize<List<T>>
            where TWrap : struct, ISerialize<T>
        {
            public static ISerdeInfo SerdeInfo => ListSerdeTypeInfo<T>.TypeInfo;
            public void Serialize(List<T> value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(ListSerdeTypeInfo<T>.TypeInfo, CollectionsMarshal.AsSpan(value), serializer);
        }

        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<List<T>>
            where TWrap : IDeserialize<T>
        {
            public static ISerdeInfo SerdeInfo => ListSerdeTypeInfo<T>.TypeInfo;
            public static List<T> Deserialize(IDeserializer deserializer)
            {
                List<T> list;
                var typeInfo = ListSerdeTypeInfo<T>.TypeInfo;
                var deCollection = deserializer.ReadCollection(typeInfo);
                if (deCollection.SizeOpt is int size)
                {
                    list = new List<T>(size);
                }
                else
                {
                    size = -1; // Set initial size to unknown
                    list = new List<T>();
                }
                while (deCollection.TryReadValue<T, TWrap>(typeInfo, out T? next))
                {
                    list.Add(next);
                }
                if (size >= 0 && list.Count != size)
                {
                    throw new DeserializeException($"Expected enumerable of size {size}, but only received {list.Count} items");
                }
                return list;
            }
        }
    }

    internal static class ImmutableArraySerdeTypeInfo<T>
    {
        public static readonly ISerdeInfo TypeInfo = SerdeInfo.MakeEnumerable(typeof(ImmutableArray<T>).ToString());
    }

    public static class ImmutableArrayWrap
    {
        public readonly struct SerializeImpl<T, TWrap> : ISerialize<ImmutableArray<T>>
            where TWrap : struct, ISerialize<T>
        {
            public static ISerdeInfo SerdeInfo => ImmutableArraySerdeTypeInfo<T>.TypeInfo;
            public void Serialize(ImmutableArray<T> value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(ImmutableArraySerdeTypeInfo<T>.TypeInfo, value.AsSpan(), serializer);
        }

        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<ImmutableArray<T>>
            where TWrap : IDeserialize<T>
        {
            public static ISerdeInfo SerdeInfo => ImmutableArraySerdeTypeInfo<T>.TypeInfo;
            public static ImmutableArray<T> Deserialize(IDeserializer deserializer)
            {
                ImmutableArray<T>.Builder builder;
                var typeInfo = ImmutableArraySerdeTypeInfo<T>.TypeInfo;
                var d = deserializer.ReadCollection(typeInfo);
                if (d.SizeOpt is int size)
                {
                    builder = ImmutableArray.CreateBuilder<T>(size);
                }
                else
                {
                    size = -1; // Set initial size to unknown
                    builder = ImmutableArray.CreateBuilder<T>();
                }

                while (d.TryReadValue<T, TWrap>(typeInfo, out T? next))
                {
                    builder.Add(next);
                }
                if (size >= 0 && builder.Count != size)
                {
                    throw new DeserializeException($"Expected {size} items, found {builder.Count}");
                }
                return builder.ToImmutable();
            }
        }
    }
}