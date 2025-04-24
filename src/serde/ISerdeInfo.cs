
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
    /// <see cref="ITypeDeserializer.IndexNotFound"/> if not found.
    /// </summary>
    int TryGetIndex(Utf8Span fieldName);

    [Experimental("SerdeExperimentalFieldInfo")]
    ISerdeInfo GetFieldInfo(int index);

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
    I8,
    I16,
    I32,
    I64,
    F32,
    F64,
    Decimal,
    String,
    DateTime,
    Bytes,
}

/// <summary>
/// Provides info for a "closed union" of types that can be serialized or deserialized.
/// </summary>
public interface IUnionSerdeInfo : ISerdeInfo
{
    InfoKind ISerdeInfo.Kind => InfoKind.Union;

    ImmutableArray<ISerdeInfo> CaseInfos { get; }
}