// Contains implementations of data interfaces for core types

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
    where TFixBuilder : class
    where TVarBuilder : class
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
                FixAdd(builder, _de.Deserialize(deCollection, info, i));
            }
            return FromFix(builder);
        }
        else
        {
            var builder = GetVarBuilder();
            int index;
            while (true)
            {
                index = deCollection.TryReadIndex(info);
                if (index == ITypeDeserializer.EndOfType)
                {
                    break;
                }

                VarAdd(builder, _de.Deserialize(deCollection, info, index));
            }
            return FromVar(builder);
        }
    }

    protected abstract TFixBuilder GetFixBuilder(int size);
    protected abstract void FixAdd(TFixBuilder builder, T item);
    protected abstract void VarAdd(TVarBuilder builder, T item);
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
    where TBuilder : class
    where TProvider : IDeserializeProvider<T>
{
    protected DeListBase(ISerdeInfo serdeInfo)
        : base(serdeInfo)
    { }

    protected abstract TBuilder GetBuilder(int? sizeOpt);
    protected sealed override TBuilder GetFixBuilder(int size) => GetBuilder(size);
    protected sealed override TBuilder GetVarBuilder() => GetBuilder(null);

    protected abstract void Add(TBuilder builder, T item);
    protected sealed override void FixAdd(TBuilder builder, T item) => Add(builder, item);
    protected sealed override void VarAdd(TBuilder builder, T item) => Add(builder, item);

    protected abstract TList ToList(TBuilder builder);
    protected sealed override TList FromFix(TBuilder builder) => ToList(builder);
    protected sealed override TList FromVar(TBuilder builder) => ToList(builder);
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
        : DeListBase<
            De<T, TProvider>,
            T,
            T[],
            De<T, TProvider>.ArrayBuilder,
            List<T>,
            TProvider
          >(ArraySerdeTypeInfo<T>.SerdeInfo),
          IDeserializeProvider<T[]>
        where TProvider : IDeserializeProvider<T>
    {
        public sealed class ArrayBuilder(int size)
        {
            private int _index = 0;
            private readonly T[] _array = new T[size];
            public void Add(T item) => _array[_index++] = item;
            public T[] ToArray() => _array;
        }
        protected override ArrayBuilder GetFixBuilder(int size) => new(size);
        protected override void FixAdd(ArrayBuilder builder, T item) => builder.Add(item);
        protected override void VarAdd(List<T> builder, T item) => builder.Add(item);
        protected override List<T> GetVarBuilder() => [];
        protected override T[] FromFix(ArrayBuilder builder) => builder.ToArray();
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

        protected override void Add(List<T> builder, T item)
        {
            builder.Add(item);
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
        protected override void Add(ImmutableArray<T>.Builder builder, T item) => builder.Add(item);
        protected override ImmutableArray<T> ToList(ImmutableArray<T>.Builder builder) => builder.ToImmutable();
    }
}