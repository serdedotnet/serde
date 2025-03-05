// Contains implementations of data interfaces for core types

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Serde;

public static class EnumerableHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeSpan<T, U>(ISerdeInfo typeInfo, ReadOnlySpan<T> arr, U serializeImpl, ISerializer serializer)
        where U : ISerialize<T>
    {
        var enumerable = serializer.WriteCollection(typeInfo, arr.Length);
        foreach (var item in arr)
        {
            enumerable.WriteElement(item, serializeImpl);
        }
        enumerable.End(typeInfo);
    }
}

public static partial class DeserializeExtensions
{
    public static ListProxy.Deserialize<T, T> GetDeserialize<T>(this List<T>? _)
        where T : IDeserializeProvider<T>
        => ListProxy.Deserialize<T, T>.Instance;
}

internal static class ArraySerdeTypeInfo<T>
{
    public static readonly ISerdeInfo SerdeInfo = Serde.SerdeInfo.MakeEnumerable(typeof(T[]).ToString());
}

public static class ArrayProxy
{
    public sealed class Serialize<T, TProvider> : SerializeInstance<T, ISerialize<T>>, ISerializeProvider<T[]>
        where TProvider : ISerializeProvider<T>
    {
        public static Serialize<T, TProvider> Instance { get; } = new Serialize<T, TProvider>();
        static ISerialize<T[]> ISerializeProvider<T[]>.SerializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => ArraySerdeTypeInfo<T>.SerdeInfo;

        private Serialize() : base(TProvider.SerializeInstance) { }
    }

    public class SerializeInstance<T, TProxy>(TProxy proxy) : ISerialize<T[]>
        where TProxy : ISerialize<T>
    {
        public void Serialize(T[] value, ISerializer serializer)
            => EnumerableHelpers.SerializeSpan<T, TProxy>(ArraySerdeTypeInfo<T>.SerdeInfo, value, proxy, serializer);
    }

    public sealed class Deserialize<T, TProvider> : DeserializeInstance<T, IDeserialize<T>>, IDeserializeProvider<T[]>
        where TProvider : IDeserializeProvider<T>
    {
        public static Deserialize<T, TProvider> Instance { get; } = new();
        public static IDeserialize<T[]> DeserializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => ArraySerdeTypeInfo<T>.SerdeInfo;

        private Deserialize() : base(TProvider.DeserializeInstance) { }
    }

    public class DeserializeInstance<T, TProxy>(TProxy proxy) : IDeserialize<T[]>
        where TProxy : IDeserialize<T>
    {
        public T[] Deserialize(IDeserializer deserializer)
        {
            var typeInfo = ArraySerdeTypeInfo<T>.SerdeInfo;
            var deCollection = deserializer.ReadCollection(typeInfo);
            if (deCollection.SizeOpt is int size)
            {
                var array = new T[size];
                for (int i = 0; i < size; i++)
                {
                    if (!deCollection.TryReadValue<T, TProxy>(typeInfo, proxy, out var value))
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
                while (deCollection.TryReadValue<T, TProxy>(typeInfo, proxy, out var value))
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

public static class ListProxy
{
    public sealed class Serialize<T, TProvider> : SerializeImpl<T, ISerialize<T>>, ISerializeProvider<List<T>>
        where TProvider : ISerializeProvider<T>
    {
        public static Serialize<T, TProvider> Instance { get; } = new Serialize<T, TProvider>();
        public static ISerialize<List<T>> SerializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => ListSerdeTypeInfo<T>.TypeInfo;

        private Serialize() : base(TProvider.SerializeInstance) { }
    }

    public class SerializeImpl<T, TProxy>(TProxy proxy) : ISerialize<List<T>>
        where TProxy : ISerialize<T>
    {
        public void Serialize(List<T> value, ISerializer serializer)
            => EnumerableHelpers.SerializeSpan<T, TProxy>(ListSerdeTypeInfo<T>.TypeInfo, CollectionsMarshal.AsSpan(value), proxy, serializer);
    }

    public sealed class Deserialize<T, TProvider> : DeserializeInstance<T, IDeserialize<T>>, IDeserializeProvider<List<T>>
        where TProvider : IDeserializeProvider<T>
    {
        public static Deserialize<T, TProvider> Instance { get; } = new();
        static IDeserialize<List<T>> IDeserializeProvider<List<T>>.DeserializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => ListSerdeTypeInfo<T>.TypeInfo;
        private Deserialize() : base(TProvider.DeserializeInstance) { }
    }

    public class DeserializeInstance<T, TProxy>(TProxy proxy) : IDeserialize<List<T>>
        where TProxy : IDeserialize<T>
    {
        public List<T> Deserialize(IDeserializer deserializer)
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
            while (deCollection.TryReadValue<T, TProxy>(typeInfo, proxy, out T? next))
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
    public static readonly ISerdeInfo SerdeInfo = Serde.SerdeInfo.MakeEnumerable(typeof(ImmutableArray<T>).ToString());
}

public static class ImmutableArrayProxy
{
    public sealed class Serialize<T, TProvider> : SerializeInstance<T, ISerialize<T>>, ISerializeProvider<ImmutableArray<T>>
        where TProvider : ISerializeProvider<T>
    {
        public static Serialize<T, TProvider> Instance { get; } = new();
        static ISerialize<ImmutableArray<T>> ISerializeProvider<ImmutableArray<T>>.SerializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => ImmutableArraySerdeTypeInfo<T>.SerdeInfo;
        private Serialize() : base(TProvider.SerializeInstance) { }
    }

    public class SerializeInstance<T, TProxy>(TProxy proxy) : ISerialize<ImmutableArray<T>>
        where TProxy : ISerialize<T>
    {
        public void Serialize(ImmutableArray<T> value, ISerializer serializer)
            => EnumerableHelpers.SerializeSpan<T, TProxy>(ImmutableArraySerdeTypeInfo<T>.SerdeInfo, value.AsSpan(), proxy, serializer);
    }

    public sealed class Deserialize<T, TProvider> : DeserializeInstance<T, IDeserialize<T>>, IDeserializeProvider<ImmutableArray<T>>
        where TProvider : IDeserializeProvider<T>
    {
        public static Deserialize<T, TProvider> Instance { get; } = new Deserialize<T, TProvider>();
        static IDeserialize<ImmutableArray<T>> IDeserializeProvider<ImmutableArray<T>>.DeserializeInstance => Instance;
        public static ISerdeInfo SerdeInfo => ImmutableArraySerdeTypeInfo<T>.SerdeInfo;
        private Deserialize() : base(TProvider.DeserializeInstance) { }
    }

    public class DeserializeInstance<T, TProxy>(TProxy proxy) : IDeserialize<ImmutableArray<T>>
        where TProxy : IDeserialize<T>
    {
        public ImmutableArray<T> Deserialize(IDeserializer deserializer)
        {
            ImmutableArray<T>.Builder builder;
            var typeInfo = ImmutableArraySerdeTypeInfo<T>.SerdeInfo;
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

            while (d.TryReadValue<T, TProxy>(typeInfo, proxy, out T? next))
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