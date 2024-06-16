
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Serde;

/// <summary>
/// TypeInfo holds a variety of indexed information about a type. The most important is a map
/// from field names to int indices. This is an optimization for deserializing types that avoids
/// allocating strings for field names.
///
/// It can also be used to get the custom attributes for a field.
/// </summary>
public sealed class TypeInfo
{
    public enum TypeKind
    {
        Primitive,
        CustomType,
        Enumerable,
        Dictionary,
    }

    public TypeKind Kind { get; }

    /// <summary>
    /// Returns an index corresponding to the location of the field in the original
    /// ReadOnlySpan passed during creation of the <see cref="TypeInfo"/>. This can be
    /// used as a fast lookup for a field based on its UTF-8 name.
    /// </summary>
    public int TryGetIndex(Utf8Span utf8FieldName)
    {
        int mapIndex = BinarySearch(_nameToIndex.AsSpan(), utf8FieldName);

        return mapIndex < 0 ? IDeserializeType.IndexNotFound : _nameToIndex[mapIndex].Index;
    }

    public IList<CustomAttributeData> GetCustomAttributeData(int index)
    {
        return _indexToInfo[index].CustomAttributesData;
    }

    /// <summary>
    /// Create a new field mapping. The ordering of the fields is important -- it
    /// corresponds to the index returned by <see cref="IDeserializeType.TryReadIndex" />.
    /// </summary>
    public static TypeInfo Create(
        TypeKind typeKind,
        ReadOnlySpan<(string SerializeName, MemberInfo MemberInfo)> fields)
    {
        var nameToIndexBuilder = ImmutableArray.CreateBuilder<(ReadOnlyMemory<byte> Utf8Name, int Index)>(fields.Length);
        var indexToInfoBuilder = ImmutableArray.CreateBuilder<PrivateFieldInfo>(fields.Length);
        for (int index = 0; index < fields.Length; index++)
        {
            var (serializeName, memberInfo) = fields[index];
            if (memberInfo is null)
            {
                throw new ArgumentNullException(serializeName);
            }

            nameToIndexBuilder.Add((s_utf8.GetBytes(serializeName), index));
            var fieldInfo = new PrivateFieldInfo(memberInfo.GetCustomAttributesData());
            indexToInfoBuilder.Add(fieldInfo);
        }

        nameToIndexBuilder.Sort((left, right) =>
            left.Utf8Name.Span.SequenceCompareTo(right.Utf8Name.Span));

        return new TypeInfo(typeKind, nameToIndexBuilder.ToImmutable(), indexToInfoBuilder.ToImmutable());
    }

    #region Private implementation details

    // The field names are sorted by the Utf8 representation of the field name.
    private readonly ImmutableArray<(ReadOnlyMemory<byte> Utf8Name, int Index)> _nameToIndex;
    private readonly ImmutableArray<PrivateFieldInfo> _indexToInfo;

    /// <summary>
    /// Holds information for a field or property in the given type.
    /// </summary>
    private readonly record struct PrivateFieldInfo(IList<CustomAttributeData> CustomAttributesData);

    private static readonly UTF8Encoding s_utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    private TypeInfo(
        TypeKind typeKind,
        ImmutableArray<(ReadOnlyMemory<byte>, int)> nameToIndex,
        ImmutableArray<PrivateFieldInfo> indexToInfo)
    {
        Kind = typeKind;
        _nameToIndex = nameToIndex;
        _indexToInfo = indexToInfo;
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

    #endregion  // Private implementation details
}
