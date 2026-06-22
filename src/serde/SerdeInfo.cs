
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Serde;

/// <summary>
/// Helper class for interacting with <see cref="ISerdeInfo"/>.
/// </summary>
public static class SerdeInfo
{
    /// <summary>
    /// Create an <see cref="ISerdeInfo"/> for a custom type. The ordering of the fields is
    /// important: it corresponds to the index returned by <see cref="ITypeDeserializer.TryReadIndex" />.
    /// </summary>
    public static ISerdeInfo MakeCustom(
        string typeName,
        IList<CustomAttributeData> typeAttributes,
        ReadOnlySpan<(string SerializeName, ISerdeInfo SerdeInfo, MemberInfo? MemberInfo)> fields)
        => MakeCustom(typeName, typeAttributes, fields, Array.Empty<int>());

    /// <summary>
    /// Create an <see cref="ISerdeInfo"/> for a custom type, assigning each field an explicit
    /// ordinal (its stable logical identity, see <see cref="ISerdeInfo.GetFieldOrdinal"/>). The
    /// ordinals are given in field order and may be sparse. Pass an empty span to leave the
    /// ordinals implicit (equal to the physical position).
    /// </summary>
    public static ISerdeInfo MakeCustom(
        string typeName,
        IList<CustomAttributeData> typeAttributes,
        ReadOnlySpan<(string SerializeName, ISerdeInfo SerdeInfo, MemberInfo? MemberInfo)> fields,
        ReadOnlySpan<int> fieldOrdinals)
    {
        var converted = new (string, ISerdeInfo, IList<CustomAttributeData>)[fields.Length];
        for (int i = 0; i < fields.Length; i++)
        {
            converted[i] = (
                fields[i].SerializeName,
                fields[i].SerdeInfo,
                fields[i].MemberInfo?.GetCustomAttributesData()
                    ?? (IList<CustomAttributeData>)Array.Empty<CustomAttributeData>());
        }
        return TypeWithFieldsInfo.Create(typeName, InfoKind.CustomType, typeAttributes, converted, fieldOrdinals);
    }

    /// <summary>
    /// Create an <see cref="ISerdeInfo"/> for a custom type with explicit field attributes.
    /// </summary>
    public static ISerdeInfo MakeCustom(
        string typeName,
        IList<CustomAttributeData> typeAttributes,
        ReadOnlySpan<(string SerializeName, ISerdeInfo SerdeInfo, IList<CustomAttributeData> FieldAttributes)> fields)
        => TypeWithFieldsInfo.Create(typeName, InfoKind.CustomType, typeAttributes, fields);

    /// <summary>
    /// Create an <see cref="ISerdeInfo"/> for a custom type without field attributes or MemberInfo.
    /// Field attributes default to empty.
    /// </summary>
    public static ISerdeInfo MakeCustom(
        string typeName,
        IList<CustomAttributeData> typeAttributes,
        ReadOnlySpan<(string SerializeName, ISerdeInfo SerdeInfo)> fields)
    {
        var converted = new (string, ISerdeInfo, IList<CustomAttributeData>)[fields.Length];
        for (int i = 0; i < fields.Length; i++)
        {
            converted[i] = (fields[i].SerializeName, fields[i].SerdeInfo, Array.Empty<CustomAttributeData>());
        }
        return TypeWithFieldsInfo.Create(typeName, InfoKind.CustomType, typeAttributes, converted);
    }

    public static ISerdeInfo MakeEnum(
        string typeName,
        IList<CustomAttributeData> typeAttributes,
        ISerdeInfo underlyingInfo,
        ReadOnlySpan<(string SerializeName, MemberInfo? MemberInfo)> fields)
    {
        var fieldsWithInfo = new (string SerializeName, ISerdeInfo SerdeInfo, IList<CustomAttributeData> FieldAttributes)[fields.Length];
        for (int i = 0; i < fields.Length; i++)
        {
            fieldsWithInfo[i] = (
                fields[i].SerializeName,
                underlyingInfo,
                fields[i].MemberInfo?.GetCustomAttributesData()
                    ?? (IList<CustomAttributeData>)Array.Empty<CustomAttributeData>());
        }

        return TypeWithFieldsInfo.Create(typeName, InfoKind.Enum, typeAttributes, fieldsWithInfo);
    }

    public static ISerdeInfo MakePrimitive(string name, PrimitiveKind kind) => new PrimitiveInfo(name, kind);

    public static ISerdeInfo MakeNullable(ISerdeInfo underlying) => new NullableSerdeInfo(underlying);

    public static ISerdeInfo MakeEnumerable(
        string typeName,
        ISerdeInfo elementInfo)
        => new CollectionInfo(typeName, InfoKind.List, [elementInfo]);

    /// <summary>
    /// Create an <see cref="ISerdeInfo"/> for a fixed-length, heterogeneous tuple. Each element
    /// becomes a field named <c>Item1</c>, <c>Item2</c>, ... in order. The ordering corresponds to
    /// the index returned by <see cref="ITypeDeserializer.TryReadIndex" />.
    /// </summary>
    public static ISerdeInfo MakeTuple(ReadOnlySpan<ISerdeInfo> itemInfos)
    {
        var sb = new System.Text.StringBuilder("ValueTuple<");
        for (int i = 0; i < itemInfos.Length; i++)
        {
            if (i > 0)
            {
                sb.Append(", ");
            }
            sb.Append(itemInfos[i].Name);
        }
        sb.Append('>');

        var fields = new (string, ISerdeInfo, IList<CustomAttributeData>)[itemInfos.Length];
        for (int i = 0; i < itemInfos.Length; i++)
        {
            fields[i] = ($"Item{i + 1}", itemInfos[i], Array.Empty<CustomAttributeData>());
        }
        return TypeWithFieldsInfo.Create(
            sb.ToString(),
            InfoKind.Tuple,
            Array.Empty<CustomAttributeData>(),
            fields);
    }

    public static ISerdeInfo MakeDictionary(
        string typeName,
        ISerdeInfo keyInfo,
        ISerdeInfo valueInfo)
        => new CollectionInfo(typeName, InfoKind.Dictionary, [keyInfo, valueInfo]);

    /// <summary>
    /// Make an ISerdeInfo that represents a union. Each case is represented as a field, meaning
    /// that each index corresponds to a case.
    /// </summary>
    public static IUnionSerdeInfo MakeUnion(
        string typeName,
        IList<CustomAttributeData> typeAttributes,
        ImmutableArray<ISerdeInfo> caseInfos)
        => new UnionSerdeInfo(typeName, typeAttributes, caseInfos);

    private sealed record PrimitiveInfo(string Name, PrimitiveKind PrimitiveKind) : INoFieldsInfo
    {
        PrimitiveKind? ISerdeInfo.PrimitiveKind => PrimitiveKind;
        public InfoKind Kind => InfoKind.Primitive;

        public IList<CustomAttributeData> Attributes => [];
    }

    private sealed record CollectionInfo(
        string Name,
        InfoKind Kind,
        ImmutableArray<ISerdeInfo> TypeArgInfos) : ISerdeInfo
    {
        public int FieldCount => TypeArgInfos.Length;

        public PrimitiveKind? PrimitiveKind => null;

        public IList<CustomAttributeData> Attributes => [];

        public Utf8Span GetFieldName(int index)
            => index >= 0 && index < TypeArgInfos.Length
                ? ISerdeInfo.UTF8Encoding.GetBytes(TypeArgInfos[index].Name)
                : throw GetOOR(index);

        public string GetFieldStringName(int index)
            => index >= 0 && index < TypeArgInfos.Length
                ? TypeArgInfos[index].Name
                : throw GetOOR(index);

        public IList<CustomAttributeData> GetFieldAttributes(int index)
            => index >= 0 && index < TypeArgInfos.Length
                ? []
                : throw GetOOR(index);

        public int TryGetIndex(Utf8Span fieldName) => ITypeDeserializer.IndexNotFound;

        public ISerdeInfo GetFieldInfo(int index)
            => index >= 0 && index < TypeArgInfos.Length
                ? TypeArgInfos[index]
                : throw GetOOR(index);

        private ArgumentOutOfRangeException GetOOR(int index)
            => new ArgumentOutOfRangeException(nameof(index), index, $"{Name} has {TypeArgInfos.Length} type argument(s).");
    }
}

public static class SerdeInfoExtensions
{
    public static ISerdeInfo WithName(this ISerdeInfo @this, string newName) => new WrappingInfo(@this, newName);
}

/// <summary>
/// Represents a nullable type (struct or class). In Serde the nullable type wrappers are unified
/// into a single wrapper. They each have a single field named "Value" which is the wrapped type and
/// their type name is the wrapped type name followed by '?'.
/// </summary>
file sealed record NullableSerdeInfo(ISerdeInfo UnderlyingInfo) : ISerdeInfo
{
    public string Name { get; } = UnderlyingInfo.Name + "?";

    public InfoKind Kind => InfoKind.Nullable;
    public PrimitiveKind? PrimitiveKind => null;
    public int FieldCount => 1;

    public IList<CustomAttributeData> Attributes => [];

    public Utf8Span GetFieldName(int index)
        => index == 0 ? "Value"u8 : throw GetOOR(index);

    public string GetFieldStringName(int index)
        => index == 0 ? "Value" : throw GetOOR(index);

    public IList<CustomAttributeData> GetFieldAttributes(int index)
        => index == 0 ? [] : throw GetOOR(index);

    public int TryGetIndex(Utf8Span fieldName) => "Value"u8.SequenceEqual(fieldName) ? 0 : ITypeDeserializer.IndexNotFound;

    public ISerdeInfo GetFieldInfo(int index) => index == 0 ? UnderlyingInfo : throw GetOOR(index);

    private ArgumentOutOfRangeException GetOOR(int index)
        => new ArgumentOutOfRangeException(nameof(index), index, $"{Name} has only one field.");
}

/// <summary>
/// Wraps an existing <see cref="ISerdeInfo"/> with a new name.
/// </summary>
file sealed class WrappingInfo(ISerdeInfo underlying, string name) : ISerdeInfo
{
    public string Name => name;
    public InfoKind Kind => underlying.Kind;
    public PrimitiveKind? PrimitiveKind => underlying.PrimitiveKind;
    public IList<CustomAttributeData> Attributes => underlying.Attributes;
    public int FieldCount => underlying.FieldCount;

    public Utf8Span GetFieldName(int index) => underlying.GetFieldName(index);
    public string GetFieldStringName(int index) => underlying.GetFieldStringName(index);
    public IList<CustomAttributeData> GetFieldAttributes(int index) => underlying.GetFieldAttributes(index);
    public int TryGetIndex(Utf8Span fieldName) => underlying.TryGetIndex(fieldName);
    public int GetFieldOrdinal(int fieldPosition) => underlying.GetFieldOrdinal(fieldPosition);
    public bool HasExplicitFieldOrdinals => underlying.HasExplicitFieldOrdinals;

    [Experimental("SerdeExperimentalFieldInfo")]
#pragma warning disable SerdeExperimentalFieldInfo
    public ISerdeInfo GetFieldInfo(int index) => underlying.GetFieldInfo(index);
#pragma warning restore SerdeExperimentalFieldInfo
}


file interface INoFieldsInfo : ISerdeInfo
{
    int ISerdeInfo.FieldCount => 0;

    Utf8Span ISerdeInfo.GetFieldName(int index)
        => throw GetOOR(index);
    string ISerdeInfo.GetFieldStringName(int index)
        => throw GetOOR(index);
    IList<CustomAttributeData> ISerdeInfo.GetFieldAttributes(int index)
        => throw GetOOR(index);

    int ISerdeInfo.TryGetIndex(Utf8Span fieldName) => ITypeDeserializer.IndexNotFound;

    ISerdeInfo ISerdeInfo.GetFieldInfo(int index)
        => throw GetOOR(index);

    private ArgumentOutOfRangeException GetOOR(int index)
        => new ArgumentOutOfRangeException(nameof(index), index, $"{Name} has no fields or properties.");
}

/// <summary>
/// <see cref="TypeWithFieldsInfo"/> holds a variety of indexed information about a custom type. The
/// most important is a map from field names to int indices. This is an optimization for
/// deserializing types that avoids allocating strings for field names.
///
/// It can also be used to get the custom attributes for a field.
/// </summary>
file sealed record TypeWithFieldsInfo : ISerdeInfo
{
    // The field names are sorted by the Utf8 representation of the field name.
    private readonly ImmutableArray<(ReadOnlyMemory<byte> Utf8Name, int Index)> _nameToIndex;
    private readonly ImmutableArray<PrivateFieldInfo> _indexToInfo;
    private readonly bool _hasExplicitOrdinals;

    /// <summary>
    /// Holds information for a field or property in the given type.
    /// </summary>
    private readonly record struct PrivateFieldInfo(
        string StringName,
        ReadOnlyMemory<byte> Utf8Name,
        IList<CustomAttributeData> CustomAttributesData,
        ISerdeInfo FieldSerdeInfo,
        int Ordinal);

    public string Name { get; }

    public InfoKind Kind { get; }

    public PrimitiveKind? PrimitiveKind => null;

    public IList<CustomAttributeData> Attributes { get; }


    private TypeWithFieldsInfo(
        string typeName,
        InfoKind typeKind,
        IList<CustomAttributeData> typeAttributes,
        ImmutableArray<(ReadOnlyMemory<byte>, int)> nameToIndex,
        ImmutableArray<PrivateFieldInfo> indexToInfo,
        bool hasExplicitOrdinals)
    {
        Name = typeName;
        Kind = typeKind;
        Attributes = typeAttributes;
        _nameToIndex = nameToIndex;
        _indexToInfo = indexToInfo;
        _hasExplicitOrdinals = hasExplicitOrdinals;
    }

    /// <summary>
    /// Create a new field mapping. The ordering of the fields is important -- it
    /// corresponds to the index returned by <see cref="ITypeDeserializer.TryReadIndex" />.
    /// </summary>
    public static TypeWithFieldsInfo Create(
        string typeName,
        InfoKind typeKind,
        IList<CustomAttributeData> typeAttributes,
        ReadOnlySpan<(string SerializeName, ISerdeInfo SerdeInfo, IList<CustomAttributeData> FieldAttributes)> fields
    ) => Create(typeName, typeKind, typeAttributes, fields, Array.Empty<int>());

    /// <summary>
    /// Create a new field mapping. The ordering of the fields is important -- it
    /// corresponds to the index returned by <see cref="ITypeDeserializer.TryReadIndex" />.
    /// </summary>
    public static TypeWithFieldsInfo Create(
        string typeName,
        InfoKind typeKind,
        IList<CustomAttributeData> typeAttributes,
        ReadOnlySpan<(string SerializeName, ISerdeInfo SerdeInfo, IList<CustomAttributeData> FieldAttributes)> fields,
        ReadOnlySpan<int> fieldOrdinals)
    {
        var nameToIndexBuilder = ImmutableArray.CreateBuilder<(ReadOnlyMemory<byte> Utf8Name, int Index)>(fields.Length);
        var indexToInfoBuilder = ImmutableArray.CreateBuilder<PrivateFieldInfo>(fields.Length);
        for (int index = 0; index < fields.Length; index++)
        {
            var (serializeName, serdeInfo, fieldAttributes) = fields[index];

            nameToIndexBuilder.Add((ISerdeInfo.UTF8Encoding.GetBytes(serializeName), index));
            var fieldInfo = new PrivateFieldInfo(
                serializeName,
                default,
                fieldAttributes,
                serdeInfo,
                fieldOrdinals.IsEmpty ? index : fieldOrdinals[index]);
            indexToInfoBuilder.Add(fieldInfo);
        }

        nameToIndexBuilder.Sort((left, right) =>
            left.Utf8Name.Span.SequenceCompareTo(right.Utf8Name.Span));

        for (int i = 0; i < nameToIndexBuilder.Count; i++)
        {
            var index = nameToIndexBuilder[i].Index;
            indexToInfoBuilder[index] = indexToInfoBuilder[index] with { Utf8Name = nameToIndexBuilder[i].Utf8Name };
        }

        return new TypeWithFieldsInfo(
            typeName,
            typeKind,
            typeAttributes,
            nameToIndexBuilder.ToImmutable(),
            indexToInfoBuilder.ToImmutable(),
            !fieldOrdinals.IsEmpty);
    }

    /// <summary>
    /// The number of serializable or deserializable fields or properties on the type.
    /// </summary>
    public int FieldCount => _nameToIndex.Length;

    /// <summary>
    /// Returns an index corresponding to the location of the field in the original
    /// ReadOnlySpan passed during creation of the <see cref="TypeWithFieldsInfo"/>. This can be
    /// used as a fast lookup for a field based on its UTF-8 name.
    /// </summary>
    public int TryGetIndex(Utf8Span utf8FieldName)
    {
        int mapIndex = BinarySearch(_nameToIndex.AsSpan(), utf8FieldName);

        return mapIndex < 0 ? ITypeDeserializer.IndexNotFound : _nameToIndex[mapIndex].Index;
    }

    [Experimental("SerdeExperimentalFieldInfo")]
    public ISerdeInfo GetFieldInfo(int index) => _indexToInfo[index].FieldSerdeInfo;

    public int GetFieldOrdinal(int fieldPosition) => _indexToInfo[fieldPosition].Ordinal;

    public bool HasExplicitFieldOrdinals => _hasExplicitOrdinals;

    public IList<CustomAttributeData> GetFieldAttributes(int index)
    {
        return _indexToInfo[index].CustomAttributesData;
    }

    public Utf8Span GetFieldName(int index)
    {
        return _indexToInfo[index].Utf8Name.Span;
    }

    public string GetFieldStringName(int index)
    {
        return _indexToInfo[index].StringName;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int BinarySearch(ReadOnlySpan<(ReadOnlyMemory<byte>, int)> span, Utf8Span fieldName)
    {
        return BinarySearch(ref MemoryMarshal.GetReference(span), span.Length, fieldName);
    }

    // This is a copy of the BinarySearch method from System.MemoryExtensions.
    // We can't use that version because ref structs can't yet be substituted for type arguments.
    private static int BinarySearch(ref (ReadOnlyMemory<byte> Utf8Name, int) spanStart, int length, Utf8Span fieldName)
    {
        int lo = 0;
        int hi = length - 1;
        // If length == 0, hi == -1, and loop will not be entered
        while (lo <= hi)
        {
            // PERF: `lo` or `hi` will never be negative inside the loop,
            //       so computing median using uints is safe since we know
            //       `length <= int.MaxValue`, and indices are >= 0
            //       and thus cannot overflow an uint.
            //       Saves one subtraction per loop compared to
            //       `int i = lo + ((hi - lo) >> 1);`
            int i = (int)(((uint)hi + (uint)lo) >> 1);

            int c = fieldName.SequenceCompareTo(Unsafe.Add(ref spanStart, i).Utf8Name.Span);
            if (c == 0)
            {
                return i;
            }
            else if (c > 0)
            {
                lo = i + 1;
            }
            else
            {
                hi = i - 1;
            }
        }
        // If none found, then a negative number that is the bitwise complement
        // of the index of the next element that is larger than or, if there is
        // no larger element, the bitwise complement of `length`, which
        // is `lo` at this point.
        return ~lo;
    }
}

file sealed class UnionSerdeInfo(
    string name,
    IList<CustomAttributeData> attributes,
    ImmutableArray<ISerdeInfo> caseInfos) : IUnionSerdeInfo
{
    public string Name => name;
    public IList<CustomAttributeData> Attributes => attributes;
    public ImmutableArray<ISerdeInfo> CaseInfos => caseInfos;
    public int FieldCount => caseInfos.Length;

    public IList<CustomAttributeData> GetFieldAttributes(int index)
        => caseInfos[index].Attributes;

    public ISerdeInfo GetFieldInfo(int index) => caseInfos[index];

    /// <summary>
    /// The field name for a union is the name of the case.
    /// </summary>
    public Utf8Span GetFieldName(int index) => ISerdeInfo.UTF8Encoding.GetBytes(GetFieldStringName(index));

    public string GetFieldStringName(int index) => caseInfos[index].Name;

    public int TryGetIndex(Utf8Span fieldName)
    {
        // Simple linear search since unions are expected to be small.
        var stringName = ISerdeInfo.UTF8Encoding.GetString(fieldName);
        for (int i = 0; i < caseInfos.Length; i++)
        {
            if (caseInfos[i].Name.Equals(stringName, StringComparison.Ordinal))
            {
                return i;
            }
        }
        return ITypeDeserializer.IndexNotFound;
    }
}