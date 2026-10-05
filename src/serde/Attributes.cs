using System;
using System.Diagnostics;

namespace Serde;

// Silence warnings about references to Serde types that aren't referenced by the generator
#pragma warning disable CS1574

/// <summary>
/// Generates an implementation of <see cref="Serde.ISerialize" />.
/// </summary>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum,
    AllowMultiple = false,
    Inherited = false
)]
[Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
#if !SRCGEN
public
#else
internal
#endif
sealed class GenerateSerialize : Attribute
{
    /// <summary>
    /// If non-null, the name of the type being serialized or deserialized.
    /// This is used to implement serialization and deserialization for a proxy type.
    /// </summary>
    public Type? ForType { get; init; }

    /// <summary>
    /// If non-null, serialize the declaring type <em>as</em> the given type by going through a
    /// user-defined conversion. The generated implementation converts the declaring type to this
    /// type and serializes the result. A user-defined conversion from the declaring type to this
    /// type must exist.
    /// </summary>
    public Type? As { get; init; }

    /// <summary>
    /// Enums only: serialize the enum as its underlying integral value instead of by name.
    /// </summary>
    public bool AsUnderlying { get; init; }
}

/// <summary>
/// Generates an implementation of <see cref="Serde.IDeserialize" />.
/// </summary>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum,
    AllowMultiple = false,
    Inherited = false
)]
[Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
#if !SRCGEN
public
#else
internal
#endif
sealed class GenerateDeserialize : Attribute
{
    /// <summary>
    /// If non-null, the name of the type being serialized or deserialized.
    /// This is used to implement serialization and deserialization for a proxy type.
    /// </summary>
    public Type? ForType { get; init; }

    /// <summary>
    /// If non-null, deserialize the declaring type <em>as</em> the given type by going through a
    /// user-defined conversion. The generated implementation deserializes this type and converts
    /// the result to the declaring type. A user-defined conversion from this type to the declaring
    /// type must exist.
    /// </summary>
    public Type? As { get; init; }

    /// <summary>
    /// Enums only: deserialize the enum from its underlying integral value instead of by name.
    /// </summary>
    public bool AsUnderlying { get; init; }
}

/// <summary>
/// Generates an implementation of both <see cref="Serde.ISerialize" /> and <see cref="Serde.IDeserialize" />.
/// </summary>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum,
    AllowMultiple = false,
    Inherited = false
)]
[Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
#if !SRCGEN
public
#else
internal
#endif
sealed class GenerateSerde : Attribute
{
    /// <summary>
    /// If non-null, the name of the type being serialized or deserialized.
    /// This is used to implement serialization and deserialization for a proxy type.
    /// </summary>
    public Type? ForType { get; init; }

    /// <summary>
    /// Override the generated <see cref="ISerde{T}"/> object with a custom one.
    /// </summary>
    public Type? With { get; init; }

    /// <summary>
    /// If non-null, serialize and deserialize the declaring type <em>as</em> the given type by
    /// going through user-defined conversions. The generated implementation converts the declaring
    /// type to this type when serializing and back when deserializing. User-defined conversions in
    /// both directions between the declaring type and this type must exist.
    /// </summary>
    public Type? As { get; init; }

    /// <summary>
    /// Enums only: serialize and deserialize the enum as its underlying integral value instead of
    /// by name.
    /// </summary>
    public bool AsUnderlying { get; init; }
}

/// <summary>
/// Set options for the Serde source generator for the current type.
/// </summary>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum,
    AllowMultiple = false,
    Inherited = false
)]
#if !SRCGEN
public
#else
internal
#endif
sealed class SerdeTypeOptions : Attribute
{
    /// <summary>
    /// If non-null, the type of the proxy to use for serialization and deserialization.
    /// </summary>
    public Type? Proxy { get; init; }

    /// <summary>
    /// Throw an exception during deserialization if any members not expected by the current
    /// type are present.
    /// </summary>
    public bool DenyUnknownMembers { get; init; } = false;

    /// <summary>
    /// Allow duplicate keys during deserialization.
    /// When true, duplicate keys will overwrite previous values.
    /// When false, duplicate keys will throw an exception.
    /// </summary>
    public bool AllowDuplicateKeys { get; init; } = false;

    /// <summary>
    /// Override the formatting for members.
    /// </summary>
    public MemberFormat MemberFormat { get; init; } = MemberFormat.CamelCase;

    /// <summary>
    /// The default behavior for null is to skip serialization. Set this to true to force
    /// serialization.
    /// </summary>
    public bool SerializeNull { get; init; } = false;

    /// <summary>
    /// Use the given name instead of the type name.
    /// </summary>
    public string? Rename { get; init; } = null;
}

/// <summary>
/// Set options for the Serde source generator specific to the current member.
/// </summary>
[AttributeUsage(
    AttributeTargets.Field | AttributeTargets.Property,
    AllowMultiple = false,
    Inherited = false
)]
#if !SRCGEN
public
#else
internal
#endif
sealed class SerdeMemberOptions : Attribute
{
    /// <summary>
    /// Throw an exception if the target field is not present when deserializing. When this option
    /// isn't set, a missing member is an error only if its type is non-nullable and it has no
    /// initializer that Serde can preserve. A missing member with a preserved initializer keeps
    /// the initializer's value, and a missing nullable member without one is set to null.
    /// </summary>
    public bool ThrowIfMissing { get; init; } = true;

    /// <summary>
    /// Use the given name instead of the name of the field or property.
    /// </summary>
    public string? Rename { get; init; } = null;

    /// <summary>
    /// Set an explicit ordinal for this member. The ordinal is the stable logical identity of the
    /// member, exposed as <see cref="Serde.ISerdeInfo.GetFieldOrdinal(int)"/>. Some formats (e.g. a
    /// compact array/offset-based or protobuf-style encoding) use it to encode members by position
    /// or tag rather than by name; name-based formats such as JSON ignore it.
    ///
    /// A negative value (the default) means no explicit ordinal is assigned. When any member of a
    /// type specifies an explicit ordinal, every member must specify one. Ordinals must be
    /// non-negative and unique, but they need not be contiguous: gaps ("holes") are allowed, which
    /// lets an obsolete member's ordinal be retired without renumbering the others. Members are
    /// ordered by ascending ordinal in the generated field layout.
    /// </summary>
    public int Ordinal { get; init; } = -1;

    /// <summary>
    /// If true, the source generator will pass down the attributes from the member to the
    /// serializer.
    /// </summary>
    public bool ProvideAttributes { get; init; } = false;

    /// <summary>
    /// The default behavior for null is to skip serialization. Set this to true to force
    /// serialization.
    /// </summary>
    public bool SerializeNull { get; init; } = false;

    /// <summary>
    /// Skip both serialization and deserialization of this member.
    /// </summary>
    public bool Skip { get; init; } = false;

    /// <summary>
    /// Skip serialization of this member.
    /// </summary>
    public bool SkipSerialize { get; init; } = false;

    /// <summary>
    /// Skip deserialization of this member.
    /// </summary>
    public bool SkipDeserialize { get; init; } = false;

    /// <summary>
    /// Capture members not recognized by the type into this member during deserialization, and
    /// write its entries back out as members during serialization. This is the alternative to <see
    /// cref="SerdeTypeOptions.DenyUnknownMembers"/>, which rejects unknown members, and the
    /// default, which skips them.
    ///
    /// The member must be a <c>Dictionary&lt;string, T&gt;</c>, keyed by the member names as they
    /// appear in the format. Each value is read and written by <c>T</c>'s ordinary implementation.
    /// Duplicate unknown members follow <see cref="SerdeTypeOptions.AllowDuplicateKeys"/>, like
    /// the type's own members. If there are no unknown members, a nullable member is left null and
    /// a non-nullable one gets an empty dictionary.
    ///
    /// This member is not itself serialized as a member and does not appear in the type's <see
    /// cref="ISerdeInfo"/>. Writing the captured members requires a format that supports writing
    /// fields by name (see <see cref="ITypeSerializer.WriteFieldStart(ISerdeInfo, string)"/>).
    /// </summary>
    public bool CaptureUnknownMembers { get; init; } = false;

    /// <summary>
    /// Proxy type for the ISerialize and IDeserialize implementations.
    /// </summary>
    public Type? Proxy { get; init; } = null;

    /// <summary>
    /// Proxy type for the ISerialize implementation.
    /// </summary>
    public Type? SerializeProxy { get; init; } = null;

    /// <summary>
    /// Proxy type for the IDeserialize implementation.
    /// </summary>
    public Type? DeserializeProxy { get; init; } = null;

    /// <summary>
    /// The name of the type parameter to use for the proxy type.  This is different from <see
    /// cref="Proxy" /> in that it is only used for type parameters, which can't appear as `typeof`
    /// arguments to attributes in C#.
    /// </summary>
    public string? TypeParameterProxy { get; init; } = null;

    /// <summary>
    /// The name of the type parameter to use for the proxy type.  This is different from <see
    /// cref="Proxy" /> in that it is only used for type parameters, which can't appear as `typeof`
    /// arguments to attributes in C#.
    /// </summary>
    public string? SerializeTypeParameterProxy { get; init; } = null;

    /// <summary>
    /// The name of the type parameter to use for the proxy type.  This is different from <see
    /// cref="Proxy" /> in that it is only used for type parameters, which can't appear as `typeof`
    /// arguments to attributes in C#.
    /// </summary>
    public string? DeserializeTypeParameterProxy { get; init; } = null;
}

/// <summary>
/// A enumeration of all possible types of name formatting that the source generator
/// can generate.
/// </summary>
#if !SRCGEN
public
#else
internal
#endif
enum MemberFormat : byte
{
    /// <summary>
    /// "camelCase"
    /// </summary>
    CamelCase,

    /// <summary>
    /// Use the original name of the member.
    /// </summary>
    None,

    /// <summary>
    /// "PascalCase"
    /// </summary>
    PascalCase,

    /// <summary>
    /// "kebab-case"
    /// </summary>
    KebabCase,

    /// <summary>
    /// "snake_case"
    /// </summary>
    SnakeCase,
}

[AttributeUsage(
    AttributeTargets.Class
        | AttributeTargets.Struct
        | AttributeTargets.Property
        | AttributeTargets.Field,
    AllowMultiple = true,
    Inherited = false
)]
#if !SRCGEN
public
#else
internal
#endif
sealed class UseProxy : Attribute
{
    public required Type ForType { get; init; }

    /// <summary>
    /// Proxy type for both ISerialize and IDeserialize implementations.
    /// </summary>
    public Type? Proxy { get; init; }

    /// <summary>
    /// Proxy type for the ISerialize implementation.
    /// </summary>
    public Type? SerializeProxy { get; init; }

    /// <summary>
    /// Proxy type for the IDeserialize implementation.
    /// </summary>
    public Type? DeserializeProxy { get; init; }
}
