// Contains implementations of data interfaces for core types

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Serde
{
    public static class EnumerableHelpers
    {
        public static void SerializeSpan<T, U>(string typeName, ReadOnlySpan<T> arr, U serializeImpl, ISerializer serializer)
            where U : ISerialize<T>
        {
            var enumerable = serializer.SerializeEnumerable(typeName, arr.Length);
            foreach (var item in arr)
            {
                enumerable.SerializeElement(item, serializeImpl);
            }
            enumerable.End();
        }

        public static void SerializeSpan<T, TWrap>(string typeName, ReadOnlySpan<T> arr, ISerializer serializer)
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize, ISerialize<T>
        {
            var enumerable = serializer.SerializeEnumerable(typeName, arr.Length);
            foreach (var item in arr)
            {
                enumerable.SerializeElement(TWrap.Create(item));
            }
            enumerable.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeList<T, TWrap>(string typeName, List<T> list, ISerializer serializer)
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize, ISerialize<T>
        {
            var enumerable = serializer.SerializeEnumerable(typeName, list.Count);
            foreach (var item in list)
            {
                enumerable.SerializeElement(TWrap.Create(item));
            }
            enumerable.End();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeIList<T, TWrap>(string typeName, IList<T> list, ISerializer serializer)
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
        {
            var enumerable = serializer.SerializeEnumerable(typeName, list.Count);
            foreach (var item in list)
            {
                enumerable.SerializeElement(TWrap.Create(item));
            }
            enumerable.End();
        }
    }

    internal static class ArraySerdeTypeInfo<T>
    {
        public static readonly TypeInfo TypeInfo = TypeInfo.Create(typeof(T[]).Name, TypeInfo.TypeKind.Enumerable, []);
    }

    public static class ArrayWrap
    {
        public readonly record struct SerializeImpl<T, TWrap>(T[] Value)
            : ISerialize, ISerialize<T[]>, ISerializeWrap<T[], SerializeImpl<T, TWrap>>
           where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize, ISerialize<T>
        {
            public static SerializeImpl<T, TWrap> Create(T[] t) => new SerializeImpl<T, TWrap>(t);

            public void Serialize(T[] value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(typeof(T[]).ToString(), value, serializer);

            public void Serialize(ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(typeof(T[]).ToString(), Value, serializer);
        }

        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<T[]>
            where TWrap : IDeserialize<T>
        {
            public static T[] Deserialize(IDeserializer deserializer)
            {
                var typeInfo = ArraySerdeTypeInfo<T>.TypeInfo;
                var deCollection = deserializer.DeserializeCollection(typeInfo);
                if (deCollection.SizeOpt is int size)
                {
                    var array = new T[size];
                    for (int i = 0; i < size; i++)
                    {
                        if (!deCollection.TryReadValue<T, TWrap>(typeInfo, out var value))
                        {
                            throw new InvalidDeserializeValueException($"Expected array of size {size}, but only received {i} items");
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
        public static readonly TypeInfo TypeInfo = TypeInfo.Create(typeof(List<T>).Name, TypeInfo.TypeKind.Enumerable, []);
    }

    public static class ListWrap
    {
        public readonly record struct SerializeImpl<T, TWrap>(List<T> Value)
            : ISerialize, ISerialize<List<T>>, ISerializeWrap<List<T>, SerializeImpl<T, TWrap>>
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize<T>
        {
            public static SerializeImpl<T, TWrap> Create(List<T> t) => new SerializeImpl<T, TWrap>(t);

            public void Serialize(ISerializer serializer)
                => EnumerableHelpers.SerializeList<T, TWrap>(typeof(List<T>).ToString(), Value, serializer);

            public void Serialize(List<T> value, ISerializer serializer)
                => EnumerableHelpers.SerializeList<T, TWrap>(typeof(List<T>).ToString(), value, serializer);
        }

        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<List<T>>
            where TWrap : IDeserialize<T>
        {
            public static List<T> Deserialize(IDeserializer deserializer)
            {
                List<T> list;
                var typeInfo = ListSerdeTypeInfo<T>.TypeInfo;
                var deCollection = deserializer.DeserializeCollection(typeInfo);
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
                    throw new InvalidDeserializeValueException($"Expected enumerable of size {size}, but only received {list.Count} items");
                }
                return list;
            }
        }
    }

    internal static class ImmutableArraySerdeTypeInfo<T>
    {
        public static readonly TypeInfo TypeInfo = TypeInfo.Create(
            typeof(ImmutableArray<T>).Name, TypeInfo.TypeKind.Enumerable, []);
    }

    public static class ImmutableArrayWrap
    {
        public readonly record struct SerializeImpl<T, TWrap>(ImmutableArray<T> Value)
            : ISerialize, ISerialize<ImmutableArray<T>>, ISerializeWrap<ImmutableArray<T>, SerializeImpl<T, TWrap>>
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize, ISerialize<T>
        {
            public static SerializeImpl<T, TWrap> Create(ImmutableArray<T> t) => new SerializeImpl<T, TWrap>(t);

            public void Serialize(ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(typeof(ImmutableArray<T>).ToString(), Value.AsSpan(), serializer);

            public void Serialize(ImmutableArray<T> value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(typeof(ImmutableArray<T>).ToString(), value.AsSpan(), serializer);
        }

        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<ImmutableArray<T>>
            where TWrap : IDeserialize<T>
        {
            public static ImmutableArray<T> Deserialize(IDeserializer deserializer)
            {
                ImmutableArray<T>.Builder builder;
                var typeInfo = ImmutableArraySerdeTypeInfo<T>.TypeInfo;
                var d = deserializer.DeserializeCollection(typeInfo);
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
                    throw new InvalidDeserializeValueException($"Expected {size} items, found {builder.Count}");
                }
                return builder.ToImmutable();
            }
        }
    }
}