
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Serde;

/// <summary>
/// Provides info for an arbitrary type that can be serialized or deserialized.
/// </summary>
public interface ISerdeInfo
{
    string Name { get; }

    InfoKind Kind { get; }

    /// <summary>
    /// Get the attributes for the type. This list may be modified from the original set of attributes
    /// in source code or metadata to reflect only the attributes that are relevant to serialization or
    /// deserialization.
    /// </summary>
    IList<CustomAttributeData> Attributes { get; }

    /// <summary>
    /// The number of serializable or deserializable fields or properties on the type.
    /// </summary>
    int FieldCount { get; }

    /// <summary>
    /// Get the field name as a string for the field at the given index. The index must be valid.
    /// </summary>
    string GetFieldStringName(int index);

    /// <summary>
    /// Get the field name as a UTF8 string for the field at the given index. The index must be valid.
    /// </summary>
    Utf8Span GetFieldName(int index);

    /// <summary>
    /// Get the attributes for the field at the given index. The index must be valid. This list may be
    /// modified from the original set of attributes in source code or metadata to reflect only the
    /// attributes that are relevant to serialization or deserialization.
    /// </summary>
    IList<CustomAttributeData> GetFieldAttributes(int index);

    /// <summary>
    /// Search the fields for one with the given name and return its index. Returns
    /// <see cref="IDeserializeType.IndexNotFound"/> if not found.
    /// </summary>
    int TryGetIndex(Utf8Span fieldName);

    [Experimental("SerdeExperimentalFieldInfo")]
    ISerdeInfo GetFieldInfo(int index);

}

public enum InfoKind
{
    Primitive,
    CustomType,
    Enumerable,
    Dictionary,
    Enum,
    /// <summary>
    /// Represents a closed union of types. Any type that returns this value from <see
    /// cref="ISerdeInfo.Kind"/> must also implement <see cref="IUnionSerdeInfo"/>.
    /// </summary>
    Union,
    /// <summary>
    /// Only used for skipping a value during deserialization.
    /// </summary>
    Skip
}

/// <summary>
/// Provides info for a "closed union" of types that can be serialized or deserialized.
/// </summary>
public interface IUnionSerdeInfo : ISerdeInfo
{
    InfoKind ISerdeInfo.Kind => InfoKind.Union;

    IEnumerable<ISerdeInfo> CaseInfos { get; }
}


/// <summary>
/// Helper class for interacting with <see cref="ISerdeInfo"/>.
/// </summary>
public static class SerdeInfo
{
    /// <summary>
    /// Create an <see cref="ISerdeInfo"/> for a custom type. The ordering of the fields is
    /// important: it corresponds to the index returned by <see cref="IDeserializeType.TryReadIndex" />.
    /// </summary>
    public static ISerdeInfo MakeCustom(
        string typeName,
        IList<CustomAttributeData> typeAttributes,
        ReadOnlySpan<(string SerializeName, ISerdeInfo SerdeInfo, MemberInfo MemberInfo)> fields)
        => TypeWithFieldsInfo.Create(typeName, InfoKind.CustomType, typeAttributes, fields);

    public static ISerdeInfo MakeEnum(
        string typeName,
        IList<CustomAttributeData> typeAttributes,
        ISerdeInfo underlyingInfo,
        ReadOnlySpan<(string SerializeName, MemberInfo MemberInfo)> fields)
    {
        var fieldsWithInfo = new (string SerializeName, ISerdeInfo SerdeInfo, MemberInfo MemberInfo)[fields.Length];
        for (int i = 0; i < fields.Length; i++)
        {
            fieldsWithInfo[i] = (fields[i].SerializeName, underlyingInfo, fields[i].MemberInfo);
        }

        return TypeWithFieldsInfo.Create(typeName, InfoKind.Enum, typeAttributes, fieldsWithInfo);
    }

    public static ISerdeInfo MakePrimitive(string typeName) => new PrimitiveInfo(typeName);

    public static ISerdeInfo MakeEnumerable(
        string typeName)
        => new CollectionInfo(typeName, InfoKind.Enumerable);
    public static ISerdeInfo MakeDictionary(
        string typeName)
        => new CollectionInfo(typeName, InfoKind.Dictionary);

    private sealed record PrimitiveInfo(string Name) : INoFieldsInfo
    {
        public InfoKind Kind => InfoKind.Primitive;

        public IList<CustomAttributeData> Attributes => [];
    }

    private sealed record CollectionInfo(
        string Name,
        InfoKind Kind) : INoFieldsInfo
    {
        public int FieldCount => 0;

        public IList<CustomAttributeData> Attributes => [];
    }

    private interface INoFieldsInfo : ISerdeInfo
    {
        int ISerdeInfo.FieldCount => 0;

        Utf8Span ISerdeInfo.GetFieldName(int index)
            => throw GetOOR(index);
        string ISerdeInfo.GetFieldStringName(int index)
            => throw GetOOR(index);
        IList<CustomAttributeData> ISerdeInfo.GetFieldAttributes(int index)
            => throw GetOOR(index);

        int ISerdeInfo.TryGetIndex(Utf8Span fieldName) => IDeserializeType.IndexNotFound;

        ISerdeInfo ISerdeInfo.GetFieldInfo(int index)
            => throw GetOOR(index);

        private ArgumentOutOfRangeException GetOOR(int index)
            => new ArgumentOutOfRangeException(nameof(index), index, $"{Name} has no fields or properties.");
    }
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

    /// <summary>
    /// Holds information for a field or property in the given type.
    /// </summary>
    private readonly record struct PrivateFieldInfo(
        string StringName,
        int Utf8NameIndex,
        IList<CustomAttributeData> CustomAttributesData,
        ISerdeInfo FieldSerdeInfo);

    public string Name { get; }

    public InfoKind Kind { get; }

    public IList<CustomAttributeData> Attributes { get; }


    private TypeWithFieldsInfo(
        string typeName,
        InfoKind typeKind,
        IList<CustomAttributeData> typeAttributes,
        ImmutableArray<(ReadOnlyMemory<byte>, int)> nameToIndex,
        ImmutableArray<PrivateFieldInfo> indexToInfo)
    {
        Name = typeName;
        Kind = typeKind;
        Attributes = typeAttributes;
        _nameToIndex = nameToIndex;
        _indexToInfo = indexToInfo;
    }

    private static readonly UTF8Encoding s_utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    /// <summary>
    /// Create a new field mapping. The ordering of the fields is important -- it
    /// corresponds to the index returned by <see cref="IDeserializeType.TryReadIndex" />.
    /// </summary>
    public static TypeWithFieldsInfo Create(
        string typeName,
        InfoKind typeKind,
        IList<CustomAttributeData> typeAttributes,
        ReadOnlySpan<(string SerializeName, ISerdeInfo SerdeInfo, MemberInfo MemberInfo)> fields)
    {
        var nameToIndexBuilder = ImmutableArray.CreateBuilder<(ReadOnlyMemory<byte> Utf8Name, int Index)>(fields.Length);
        var indexToInfoBuilder = ImmutableArray.CreateBuilder<PrivateFieldInfo>(fields.Length);
        for (int index = 0; index < fields.Length; index++)
        {
            var (serializeName, serdeInfo, memberInfo) = fields[index];

            nameToIndexBuilder.Add((s_utf8.GetBytes(serializeName), index));
            var fieldInfo = new PrivateFieldInfo(
                serializeName,
                -1,
                memberInfo.GetCustomAttributesData(),
                serdeInfo);
            indexToInfoBuilder.Add(fieldInfo);
        }

        nameToIndexBuilder.Sort((left, right) =>
            left.Utf8Name.Span.SequenceCompareTo(right.Utf8Name.Span));

        for (int i = 0; i < nameToIndexBuilder.Count; i++)
        {
            var index = nameToIndexBuilder[i].Index;
            indexToInfoBuilder[index] = indexToInfoBuilder[index] with { Utf8NameIndex = i };
        }

        return new TypeWithFieldsInfo(
            typeName,
            typeKind,
            typeAttributes,
            nameToIndexBuilder.ToImmutable(),
            indexToInfoBuilder.ToImmutable());
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

        return mapIndex < 0 ? IDeserializeType.IndexNotFound : _nameToIndex[mapIndex].Index;
    }

    [Experimental("SerdeExperimentalFieldInfo")]
    public ISerdeInfo GetFieldInfo(int index) => _indexToInfo[index].FieldSerdeInfo;

    public IList<CustomAttributeData> GetFieldAttributes(int index)
    {
        return _indexToInfo[index].CustomAttributesData;
    }

    public ReadOnlySpan<byte> GetFieldName(int index)
    {
        return _nameToIndex[_indexToInfo[index].Utf8NameIndex].Utf8Name.Span;
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