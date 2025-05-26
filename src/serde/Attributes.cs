
using System;
using System.Diagnostics;

namespace Serde;

// Silence warnings about references to Serde types that aren't referenced by the generator
#pragma warning disable CS1574

/// <summary>
/// Generates an implementation of <see cref="Serde.ISerialize" />.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
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
}

/// <summary>
/// Generates an implementation of <see cref="Serde.IDeserialize" />.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
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
}

/// <summary>
/// Generates an implementation of both <see cref="Serde.ISerialize" /> and <see cref="Serde.IDeserialize" />.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
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
}

/// <summary>
/// Set options for the Serde source generator for the current type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
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
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
#if !SRCGEN
public
#else
internal
#endif
sealed class SerdeMemberOptions : Attribute
{
    /// <summary>
    /// Throw an exception if the target field is not present when deserializing.  This is the
    /// default behavior for fields of non-nullable types, while the default behavior for nullable
    /// types is to set the field to null.
    /// </summary>
    public bool ThrowIfMissing { get; init; } = true;

    /// <summary>
    /// Use the given name instead of the name of the field or property.
    /// </summary>
    public string? Rename { get; init; } = null;

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
}