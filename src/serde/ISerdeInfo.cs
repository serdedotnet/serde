
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Serde;

/// <summary>
/// Provides info for an arbitrary type that can be serialized or deserialized.
/// </summary>
public interface ISerdeInfo
{
    string TypeName { get; }
    TypeKind Kind { get; }

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

    public enum TypeKind
    {
        Primitive,
        CustomType,
        Enumerable,
        Dictionary,
        Enum,
        /// <summary>
        /// Represents a closed union of types. Any type that returns this value from <see
        /// cref="Kind"/> must also implement <see cref="IUnionSerdeInfo"/>.
        /// </summary>
        Union
    }
}

/// <summary>
/// Provides info for a "closed union" of types that can be serialized or deserialized.
/// </summary>
public interface IUnionSerdeInfo : ISerdeInfo
{
    TypeKind ISerdeInfo.Kind => TypeKind.Union;

    IEnumerable<ISerdeInfo> CaseInfos { get; }
}