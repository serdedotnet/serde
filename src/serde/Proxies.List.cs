// Contains implementations of data interfaces for core types

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Serde;

public static class EnumerableHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerializeSpan<T>(ISerdeInfo typeInfo, ReadOnlySpan<T> arr, ITypeSerialize<T> serializeImpl, ISerializer serializer)
    {
        var enumerable = serializer.WriteCollection(typeInfo, arr.Length);
        int index = 0;
        foreach (var item in arr)
        {
            serializeImpl.Serialize(item, enumerable, typeInfo, index++);
        }
        enumerable.End(typeInfo);
    }
}

public abstract class SerListBase<TSelf, T, TList, TProvider>
    : ISerialize<TList>
    where TSelf : ISerialize<TList>, ISerializeProvider<TList>, ISerdeInfoProvider, new()
    where TProvider : ISerializeProvider<T>
{
    public static ISerialize<TList> SerializeInstance { get; } = new TSelf();

    private readonly ITypeSerialize<T> _ser;
    protected SerListBase()
    {
        var ser = TProvider.SerializeInstance;
        _ser = ser is ITypeSerialize<T> typeSer
            ? typeSer
            : new TypeSerBoxed<T>(ser);
    }

    void ISerialize<TList>.Serialize(TList value, ISerializer serializer)
        => EnumerableHelpers.SerializeSpan(SerdeInfoProvider.GetInfo<TSelf>(), GetSpan(value), _ser, serializer);

    protected abstract ReadOnlySpan<T> GetSpan(TList value);
}

public abstract class DeListBase<
    TSelf,
    T,
    TList,
    TFixBuilder,
    TVarBuilder,
    TProvider>
    : IDeserialize<TList>
    where TSelf : IDeserialize<TList>, IDeserializeProvider<TList>, new()
    where TFixBuilder : IList<T>
    where TVarBuilder : ICollection<T>
    where TProvider : IDeserializeProvider<T>
{
    public static IDeserialize<TList> DeserializeInstance { get; } = new TSelf();

    private readonly ITypeDeserialize<T> _de;

    protected DeListBase()
    {
        var de = TProvider.DeserializeInstance;
        Debug.Assert(TProvider.SerdeInfo.Kind != InfoKind.Primitive
            || de is ITypeDeserialize<T>, $"{typeof(T)} does not implement ITypeDeserialize");
        _de = de is ITypeDeserialize<T> typeDe
            ? typeDe
            : new TypeDeBoxed<T>(de);
    }

    public TList Deserialize(IDeserializer deserializer)
    {
        var typeInfo = TSelf.SerdeInfo;
        var deCollection = deserializer.ReadCollection(typeInfo);
        if (deCollection.SizeOpt is int size)
        {
            var builder = GetFixBuilder(size);
            for (int i = 0; i < size; i++)
            {
                builder[i] = _de.Deserialize(deCollection, typeInfo, i);
            }
            return FromFix(builder);
        }
        else
        {
            var builder = GetVarBuilder();
            int index;
            while ((index = deCollection.TryReadIndex(typeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                builder.Add(_de.Deserialize(deCollection, typeInfo, index));
            }
            return FromVar(builder);
        }
    }

    protected abstract TFixBuilder GetFixBuilder(int size);
    protected abstract TVarBuilder GetVarBuilder();
    protected abstract TList FromFix(TFixBuilder builder);
    protected abstract TList FromVar(TVarBuilder builder);
}

public abstract class DeListBase<
    TSelf,
    T,
    TList,
    TBuilder,
    TProvider>
    : DeListBase<TSelf, T, TList, TBuilder, TBuilder, TProvider>
    where TSelf : IDeserialize<TList>, IDeserializeProvider<TList>, new()
    where TBuilder : IList<T>
    where TProvider : IDeserializeProvider<T>
{
    protected abstract TBuilder GetBuilder(int? sizeOpt);
    protected sealed override TBuilder GetFixBuilder(int size) => GetBuilder(size);
    protected sealed override TBuilder GetVarBuilder() => GetBuilder(null);

    protected abstract TList ToList(TBuilder builder);
    protected sealed override TList FromFix(TBuilder builder) => ToList(builder);
    protected sealed override TList FromVar(TBuilder builder) => ToList(builder);
}

public static partial class DeserializeExtensions
{
    public static IDeserialize<List<T>> GetDeserialize<T>(this List<T>? _)
        where T : IDeserializeProvider<T>
        => ListProxy.De<T, T>.DeserializeInstance;
}

internal static class ArraySerdeTypeInfo<T>
{
    public static readonly ISerdeInfo SerdeInfo = Serde.SerdeInfo.MakeEnumerable(typeof(T[]).ToString());
}

public static class ArrayProxy
{
    public class Ser<T, TProvider>
        : SerListBase<Ser<T, TProvider>, T, T[], TProvider>,
          ISerializeProvider<T[]>
        where TProvider : ISerializeProvider<T>
    {
        public static ISerdeInfo SerdeInfo => ArraySerdeTypeInfo<T>.SerdeInfo;

        protected override ReadOnlySpan<T> GetSpan(T[] value) => value.AsSpan();
    }

    public sealed class De<T, TProvider>
        : DeListBase<De<T, TProvider>, T, T[], T[], List<T>, TProvider>,
          IDeserializeProvider<T[]>
        where TProvider : IDeserializeProvider<T>
    {
        public static ISerdeInfo SerdeInfo => ListSerdeTypeInfo<T>.TypeInfo;

        protected override T[] GetFixBuilder(int size) => new T[size];
        protected override List<T> GetVarBuilder() => [];
        protected override T[] FromFix(T[] builder) => builder;
        protected override T[] FromVar(List<T> builder) => builder.ToArray();
    }
}

internal static class ListSerdeTypeInfo<T>
{
    public static readonly ISerdeInfo TypeInfo = SerdeInfo.MakeEnumerable(typeof(List<T>).ToString());
}

public static class ListProxy
{
    public sealed class Ser<T, TProvider>
        : SerListBase<Ser<T, TProvider>, T, List<T>, TProvider>,
          ISerializeProvider<List<T>>
        where TProvider : ISerializeProvider<T>
    {
        public static ISerdeInfo SerdeInfo => ListSerdeTypeInfo<T>.TypeInfo;

        protected override ReadOnlySpan<T> GetSpan(List<T> value) => CollectionsMarshal.AsSpan(value);
    }

    public sealed class De<T, TProvider>
        : DeListBase<De<T, TProvider>, T, List<T>, List<T>, TProvider>,
          IDeserialize<List<T>>, IDeserializeProvider<List<T>>
        where TProvider : IDeserializeProvider<T>
    {
        public static ISerdeInfo SerdeInfo => ListSerdeTypeInfo<T>.TypeInfo;

        protected override List<T> GetBuilder(int? sizeOpt)
        {
            if (sizeOpt is int size)
            {
                return new List<T>(size);
            }
            return new List<T>();
        }

        protected override List<T> ToList(List<T> builder) => builder;
    }
}

internal static class ImmutableArraySerdeTypeInfo<T>
{
    public static readonly ISerdeInfo SerdeInfo = Serde.SerdeInfo.MakeEnumerable(typeof(ImmutableArray<T>).ToString());
}

public static class ImmutableArrayProxy
{
    public sealed class Ser<T, TProvider>
        : SerListBase<Ser<T, TProvider>, T, ImmutableArray<T>, TProvider>,
          ISerializeProvider<ImmutableArray<T>>
        where TProvider : ISerializeProvider<T>
    {
        public static ISerdeInfo SerdeInfo => ImmutableArraySerdeTypeInfo<T>.SerdeInfo;

        protected override ReadOnlySpan<T> GetSpan(ImmutableArray<T> value) => value.AsSpan();
    }

    public sealed class De<T, TProvider>
        : DeListBase<De<T, TProvider>, T, ImmutableArray<T>, ImmutableArray<T>.Builder, TProvider>,
          IDeserializeProvider<ImmutableArray<T>>
        where TProvider : IDeserializeProvider<T>
    {
        public static ISerdeInfo SerdeInfo => ImmutableArraySerdeTypeInfo<T>.SerdeInfo;

        protected override ImmutableArray<T>.Builder GetBuilder(int? sizeOpt) => sizeOpt is int size
            ? ImmutableArray.CreateBuilder<T>(size)
            : ImmutableArray.CreateBuilder<T>();
        protected override ImmutableArray<T> ToList(ImmutableArray<T>.Builder builder) => builder.ToImmutable();
    }
}