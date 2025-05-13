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
    where TSelf : ISerialize<TList>, ISerializeProvider<TList>, new()
    where TProvider : ISerializeProvider<T>
{
    public static ISerialize<TList> Instance { get; } = new TSelf();
    public ISerdeInfo SerdeInfo { get; }

    private readonly ITypeSerialize<T> _ser = TypeSerialize.GetOrBox<T, TProvider>();
    protected SerListBase(ISerdeInfo serdeInfo)
    {
        SerdeInfo = serdeInfo;
    }

    void ISerialize<TList>.Serialize(TList value, ISerializer serializer)
        => EnumerableHelpers.SerializeSpan(SerdeInfo, GetSpan(value), _ser, serializer);

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
    where TSelf : IDeserialize<TList>, new()
    where TFixBuilder : IList<T>
    where TVarBuilder : ICollection<T>
    where TProvider : IDeserializeProvider<T>
{
    public static IDeserialize<TList> Instance { get; } = new TSelf();

    public ISerdeInfo SerdeInfo { get; }

    private readonly ITypeDeserialize<T> _de = TypeDeserialize.GetOrBox<T, TProvider>();

    protected DeListBase(ISerdeInfo serdeInfo)
    {
        SerdeInfo = serdeInfo;
    }

    public TList Deserialize(IDeserializer deserializer)
    {
        var info = SerdeInfo;
        var deCollection = deserializer.ReadType(info);
        if (deCollection.SizeOpt is int size)
        {
            var builder = GetFixBuilder(size);
            for (int i = 0; i < size; i++)
            {
                builder.Add(_de.Deserialize(deCollection, info, i));
            }
            return FromFix(builder);
        }
        else
        {
            var builder = GetVarBuilder();
            int index;
            while ((index = deCollection.TryReadIndex(info, out _)) != ITypeDeserializer.EndOfType)
            {
                builder.Add(_de.Deserialize(deCollection, info, index));
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
    : DeListBase<TSelf, T, TList, TBuilder, TBuilder, TProvider>,
    IDeserializeProvider<TList>
    where TSelf : IDeserialize<TList>, new()
    where TBuilder : IList<T>
    where TProvider : IDeserializeProvider<T>
{
    protected DeListBase(ISerdeInfo serdeInfo)
        : base(serdeInfo)
    { }

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
        => ListProxy.De<T, T>.Instance;
}

internal static class ArraySerdeTypeInfo<T>
{
    public static readonly ISerdeInfo SerdeInfo = Serde.SerdeInfo.MakeEnumerable(typeof(T[]).ToString());
}

public static class ArrayProxy
{
    public class Ser<T, TProvider>()
        : SerListBase<Ser<T, TProvider>, T, T[], TProvider>(ArraySerdeTypeInfo<T>.SerdeInfo),
          ISerializeProvider<T[]>
        where TProvider : ISerializeProvider<T>
    {
        protected override ReadOnlySpan<T> GetSpan(T[] value) => value.AsSpan();
    }

    public sealed class De<T, TProvider>()
        : DeListBase<De<T, TProvider>, T, T[], T[], List<T>, TProvider>(ArraySerdeTypeInfo<T>.SerdeInfo),
          IDeserializeProvider<T[]>
        where TProvider : IDeserializeProvider<T>
    {
        protected override T[] GetFixBuilder(int size) => new T[size];
        protected override List<T> GetVarBuilder() => [];
        protected override T[] FromFix(T[] builder) => builder;
        protected override T[] FromVar(List<T> builder) => builder.ToArray();
    }
}

internal static class ListSerdeTypeInfo<T>
{
    public static readonly ISerdeInfo SerdeInfo = Serde.SerdeInfo.MakeEnumerable(typeof(List<T>).ToString());
}

public static class ListProxy
{
    public sealed class Ser<T, TProvider>()
        : SerListBase<Ser<T, TProvider>, T, List<T>, TProvider>(ListSerdeTypeInfo<T>.SerdeInfo),
          ISerializeProvider<List<T>>
        where TProvider : ISerializeProvider<T>
    {
        protected override ReadOnlySpan<T> GetSpan(List<T> value) => CollectionsMarshal.AsSpan(value);
    }

    public sealed class De<T, TProvider>()
        : DeListBase<De<T, TProvider>, T, List<T>, List<T>, TProvider>(ListSerdeTypeInfo<T>.SerdeInfo),
          IDeserialize<List<T>>, IDeserializeProvider<List<T>>
        where TProvider : IDeserializeProvider<T>
    {
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
    public sealed class Ser<T, TProvider>()
        : SerListBase<Ser<T, TProvider>, T, ImmutableArray<T>, TProvider>(ImmutableArraySerdeTypeInfo<T>.SerdeInfo),
          ISerializeProvider<ImmutableArray<T>>
        where TProvider : ISerializeProvider<T>
    {
        protected override ReadOnlySpan<T> GetSpan(ImmutableArray<T> value) => value.AsSpan();
    }

    public sealed class De<T, TProvider>()
        : DeListBase<De<T, TProvider>, T, ImmutableArray<T>, ImmutableArray<T>.Builder, TProvider>(ImmutableArraySerdeTypeInfo<T>.SerdeInfo),
          IDeserializeProvider<ImmutableArray<T>>
        where TProvider : IDeserializeProvider<T>
    {
        protected override ImmutableArray<T>.Builder GetBuilder(int? sizeOpt) => sizeOpt is int size
            ? ImmutableArray.CreateBuilder<T>(size)
            : ImmutableArray.CreateBuilder<T>();
        protected override ImmutableArray<T> ToList(ImmutableArray<T>.Builder builder) => builder.ToImmutable();
    }
}