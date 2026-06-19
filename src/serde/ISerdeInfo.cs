using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
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
    /// If the type is a primitive type, return its specific kind. Otherwise, return null.
    /// </summary>
    PrimitiveKind? PrimitiveKind { get; }

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
    /// <see cref="ITypeDeserializer.IndexNotFound"/> if not found.
    /// </summary>
    int TryGetIndex(Utf8Span fieldName);

    [Experimental("SerdeExperimentalFieldInfo")]
    ISerdeInfo GetFieldInfo(int index);

    /// <summary>
    /// Get the ordinal for the field at the given physical position. The physical position is the
    /// dense index in <c>[0, <see cref="FieldCount"/>)</c> used by all the other <c>GetField*</c>
    /// accessors. The ordinal is the stable identity of the field, which may be assigned explicitly
    /// (e.g. via <c>[SerdeMemberOptions(Ordinal = N)]</c>) and may be sparse, i.e. larger than
    /// <see cref="FieldCount"/> and with gaps between fields.
    ///
    /// By default the ordinal equals the physical position. Positional or tag-based formats (e.g. an
    /// array/offset or protobuf-style encoding) use this to place each field at a stable position,
    /// leaving holes where ordinals are skipped. Name-based formats (e.g. JSON) ignore it.
    ///
    /// Implementations guarantee that ordinals are strictly increasing in the physical position:
    /// <c>GetFieldOrdinal(i) &lt; GetFieldOrdinal(i + 1)</c> for all valid <c>i</c>. Consumers may
    /// therefore rely on the fields being laid out in ascending ordinal order.
    /// </summary>
    int GetFieldOrdinal(int fieldPosition) => fieldPosition;

    /// <summary>
    /// Whether the fields were assigned explicit, stable ordinals (e.g. via
    /// <c>[SerdeMemberOptions(Ordinal = N)]</c>). When <c>false</c>, <see cref="GetFieldOrdinal"/>
    /// still returns a value, but that value is the incidental physical position derived from
    /// declaration order, which is <em>not</em> a stable identity and can change as the type is
    /// edited. Positional or tag-based formats that encode by ordinal must check this first and
    /// only treat the ordinals as meaningful when it is <c>true</c>.
    /// </summary>
    bool HasExplicitFieldOrdinals => false;

    internal static readonly UTF8Encoding UTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
}

public enum InfoKind
{
    Primitive,
    Nullable,
    CustomType,
    List,
    Dictionary,
    Enum,
    /// <summary>
    /// Represents a closed union of types. Any type that returns this value from <see
    /// cref="ISerdeInfo.Kind"/> must also implement <see cref="IUnionSerdeInfo"/>.
    /// </summary>
    Union,
    /// <summary>
    /// Represents a fixed-length, heterogeneous sequence of values, e.g. a
    /// <see cref="System.ValueTuple"/>. Unlike <see cref="List"/>, each element may have a
    /// distinct type. On most formats this is encoded the same as a list (e.g. a JSON array).
    /// </summary>
    Tuple,
}

public enum PrimitiveKind
{
    Null,
    Bool,
    Char,
    U8,
    U16,
    U32,
    U64,
    U128,
    I8,
    I16,
    I32,
    I64,
    I128,
    F32,
    F64,
    Decimal,
    String,
    DateTime,
    Bytes,
    DateOnly,
    TimeOnly,
    F16,
}

/// <summary>
/// Provides info for a "closed union" of types that can be serialized or deserialized.
/// </summary>
public interface IUnionSerdeInfo : ISerdeInfo
{
    InfoKind ISerdeInfo.Kind => InfoKind.Union;

    PrimitiveKind? ISerdeInfo.PrimitiveKind => null;

    ImmutableArray<ISerdeInfo> CaseInfos { get; }
}