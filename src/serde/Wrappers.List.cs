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
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
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
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
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

    public static class ArrayWrap
    {
        public readonly record struct SerializeImpl<T, TWrap>(T[] Value)
            : ISerialize, ISerialize<T[]>, ISerializeWrap<T[], SerializeImpl<T, TWrap>>
           where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
        {
            public static SerializeImpl<T, TWrap> Create(T[] t) => new SerializeImpl<T, TWrap>(t);

            void ISerialize<T[]>.Serialize(T[] value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(typeof(T[]).ToString(), value, serializer);

            void ISerialize.Serialize(ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(typeof(T[]).ToString(), Value, serializer);
        }

        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<T[]>
            where TWrap : IDeserialize<T>
        {
            static T[] IDeserialize<T[]>.Deserialize(IDeserializer deserializer)
            {
                return deserializer.DeserializeEnumerable(new SerdeVisitor());
            }
            private sealed class SerdeVisitor : IDeserializeVisitor<T[]>
            {
                string IDeserializeVisitor<T[]>.ExpectedTypeName => typeof(T[]).ToString();

                T[] IDeserializeVisitor<T[]>.VisitEnumerable<D>(ref D d)
                {
                    if (d.SizeOpt is int size)
                    {
                        var array = new T[size];
                        for (int i = 0; i < size; i++)
                        {
                            if (!d.TryGetNext<T, TWrap>(out T? next))
                            {
                                throw new InvalidDeserializeValueException($"Expected enumerable of size {size}, but only received {i} items");
                            }
                            array[i] = next;
                        }
                        return array;
                    }
                    else
                    {
                        var list = new List<T>();
                        while (d.TryGetNext<T, TWrap>(out T? next))
                        {
                            list.Add(next);
                        }
                        return list.ToArray();
                    }
                }
            }
        }
    }

    public static class ListWrap
    {
        public readonly record struct SerializeImpl<T, TWrap>(List<T> Value)
            : ISerialize, ISerialize<List<T>>, ISerializeWrap<List<T>, SerializeImpl<T, TWrap>>
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
        {
            public static SerializeImpl<T, TWrap> Create(List<T> t) => new SerializeImpl<T, TWrap>(t);

            void ISerialize.Serialize(ISerializer serializer)
                => EnumerableHelpers.SerializeList<T, TWrap>(typeof(List<T>).ToString(), Value, serializer);

            void ISerialize<List<T>>.Serialize(List<T> value, ISerializer serializer)
                => EnumerableHelpers.SerializeList<T, TWrap>(typeof(List<T>).ToString(), value, serializer);
        }

        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<List<T>>
            where TWrap : IDeserialize<T>
        {
            static List<T> IDeserialize<List<T>>.Deserialize(IDeserializer deserializer)
            {
                return deserializer.DeserializeEnumerable(new SerdeVisitor());
            }
            private sealed class SerdeVisitor : IDeserializeVisitor<List<T>>
            {
                string IDeserializeVisitor<List<T>>.ExpectedTypeName => typeof(T[]).ToString();

                List<T> IDeserializeVisitor<List<T>>.VisitEnumerable<D>(ref D d)
                {
                    List<T> list;
                    if (d.SizeOpt is int size)
                    {
                        list = new List<T>(size);
                    }
                    else
                    {
                        size = -1; // Set initial size to unknown
                        list = new List<T>();
                    }
                    while (d.TryGetNext<T, TWrap>(out T? next))
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
    }

    public static class ImmutableArrayWrap
    {
        public readonly record struct SerializeImpl<T, TWrap>(ImmutableArray<T> Value)
            : ISerialize, ISerialize<ImmutableArray<T>>, ISerializeWrap<ImmutableArray<T>, SerializeImpl<T, TWrap>>
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
        {
            public static SerializeImpl<T, TWrap> Create(ImmutableArray<T> t) => new SerializeImpl<T, TWrap>(t);

            void ISerialize.Serialize(ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(typeof(ImmutableArray<T>).ToString(), Value.AsSpan(), serializer);

            void ISerialize<ImmutableArray<T>>.Serialize(ImmutableArray<T> value, ISerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap>(typeof(ImmutableArray<T>).ToString(), value.AsSpan(), serializer);
        }

        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<ImmutableArray<T>>
            where TWrap : IDeserialize<T>
        {
            static ImmutableArray<T> IDeserialize<ImmutableArray<T>>.Deserialize(IDeserializer deserializer)
            {
                return deserializer.DeserializeEnumerable(new Visitor());
            }

            private struct Visitor : IDeserializeVisitor<ImmutableArray<T>>
            {
                public string ExpectedTypeName => typeof(ImmutableArray<T>).ToString();
                ImmutableArray<T> IDeserializeVisitor<ImmutableArray<T>>.VisitEnumerable<D>(ref D d)
                {
                    ImmutableArray<T>.Builder builder;
                    if (d.SizeOpt is int size)
                    {
                        builder = ImmutableArray.CreateBuilder<T>(size);
                    }
                    else
                    {
                        size = -1; // Set initial size to unknown
                        builder = ImmutableArray.CreateBuilder<T>();
                    }

                    while (d.TryGetNext<T, TWrap>(out T? next))
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
}