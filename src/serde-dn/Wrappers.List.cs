// Contains implementations of data interfaces for core types

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Serde
{
    internal static class EnumerableHelpers
    {
        public static void SerializeSpan<
            T,
            TWrap,
            TSerializer,
            TSerializeType,
            TSerializeEnumerable,
            TSerializeDictionary
            >(ReadOnlySpan<T> arr, ref TSerializer serializer)
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable, TSerializeDictionary>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
            where TSerializeDictionary : ISerializeDictionary
        {
            var enumerable = serializer.SerializeEnumerable(arr.Length);
            foreach (var item in arr)
            {
                enumerable.SerializeElement(TWrap.Create(item));
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
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable, TSerializeDictionary>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
            where TSerializeDictionary : ISerializeDictionary
        {
            var enumerable = serializer.SerializeEnumerable(list.Count);
            foreach (var item in list)
            {
                enumerable.SerializeElement(TWrap.Create(item));
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
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable, TSerializeDictionary>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable
            where TSerializeDictionary : ISerializeDictionary
        {
            var enumerable = serializer.SerializeEnumerable(list.Count);
            foreach (var item in list)
            {
                enumerable.SerializeElement(TWrap.Create(item));
            }
            enumerable.End();
        }
    }

    public readonly record struct IdWrap<T>(T Value) : ISerialize, ISerializeWrap<T, IdWrap<T>>
        where T : ISerialize
    {
        public static IdWrap<T> Create(T t) => new IdWrap<T>(t);

        void ISerialize.Serialize<TSerializer, Type, Enumerable, Dictionary>(ref TSerializer serializer)
            => Value.Serialize<TSerializer, Type, Enumerable, Dictionary>(ref serializer);
    }

    public static class ArrayWrap
    {
        public readonly record struct SerializeImpl<T, TWrap>(T[] Value)
            : ISerialize, ISerializeWrap<T[], SerializeImpl<T, TWrap>>
           where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
        {
            public static SerializeImpl<T, TWrap> Create(T[] t) => new SerializeImpl<T, TWrap>(t);

            void ISerialize.Serialize<TSerializer, Type, Enumerable, Dictionary>(ref TSerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap, TSerializer, Type, Enumerable, Dictionary>(Value, ref serializer);
        }

        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<T[]>
            where TWrap : IDeserialize<T>
        {
            static T[] IDeserialize<T[]>.Deserialize<D>(ref D deserializer)
            {
                return deserializer.DeserializeEnumerable<T[], SerdeVisitor>(new SerdeVisitor());
            }
            private sealed class SerdeVisitor : IDeserializeVisitor<T[]>
            {
                string IDeserializeVisitor<T[]>.ExpectedTypeName => typeof(T).ToString() + "[]";

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
            : ISerialize, ISerializeWrap<List<T>, SerializeImpl<T, TWrap>>
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
        {
            public static SerializeImpl<T, TWrap> Create(List<T> t) => new SerializeImpl<T, TWrap>(t);

            void ISerialize.Serialize<TSerializer, Type, Enumerable, Dictionary>(ref TSerializer serializer)
                => EnumerableHelpers.SerializeList<T, TWrap, TSerializer, Type, Enumerable, Dictionary>(Value, ref serializer);
        }
    }

    public static class ImmutableArrayWrap
    {
        public readonly record struct SerializeImpl<T, TWrap>(ImmutableArray<T> Value)
            : ISerialize, ISerializeWrap<ImmutableArray<T>, SerializeImpl<T, TWrap>>
            where TWrap : struct, ISerializeWrap<T, TWrap>, ISerialize
        {
            public static SerializeImpl<T, TWrap> Create(ImmutableArray<T> t) => new SerializeImpl<T, TWrap>(t);

            void ISerialize.Serialize<TSerializer, Type, Enumerable, Dictionary>(ref TSerializer serializer)
                => EnumerableHelpers.SerializeSpan<T, TWrap, TSerializer, Type, Enumerable, Dictionary>(Value.AsSpan(), ref serializer);
        }

        public readonly struct DeserializeImpl<T, TWrap> : IDeserialize<ImmutableArray<T>>
            where TWrap : IDeserialize<T>
        {
            static ImmutableArray<T> IDeserialize<ImmutableArray<T>>.Deserialize<D>(ref D deserializer)
            {
                return deserializer.DeserializeEnumerable<
                    ImmutableArray<T>,
                    Visitor>(new Visitor());
            }

            private sealed class Visitor : IDeserializeVisitor<ImmutableArray<T>>
            {
                public string ExpectedTypeName => "ImmutableArray<" + typeof(T).ToString() + ">";
                ImmutableArray<T> IDeserializeVisitor<ImmutableArray<T>>.VisitEnumerable<D>(ref D d)
                {
                    var builder = d.SizeOpt is int size
                            ? ImmutableArray.CreateBuilder<T>(size)
                            : ImmutableArray.CreateBuilder<T>();

                    int i = 0;
                    while (true)
                    {
                        if (d.TryGetNext<T, TWrap>(out T? next))
                        {
                            builder.Add(next);
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (d.SizeOpt is int size2 && size2 != i)
                    {
                        throw new InvalidDeserializeValueException($"Expected {size2} items, found {i}");
                    }
                    return builder.ToImmutable();
                }
            }
        }
    }
}