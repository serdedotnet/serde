
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
    /// If non-null, the name of the member used to implement serialization. This is used to
    /// implement serialization for a wrapper type.
    /// </summary>
    public string? ThroughMember { get; init; }

    /// <summary>
    /// If non-null, the name of the type used to implement serialization and deserialization.
    /// This is used to implement serialization and deserialization for a wrapper type.
    /// </summary>
    public Type? ThroughType { get; init; }
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
    /// If non-null, the name of the member used to implement deserialization. This is used to
    /// implement deserialization for a wrapper type.
    /// </summary>
    public string? ThroughMember { get; init; }

    /// <summary>
    /// If non-null, the name of the type used to implement serialization and deserialization.
    /// This is used to implement serialization and deserialization for a wrapper type.
    /// </summary>
    public Type? ThroughType { get; init; }
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
    /// If non-null, the name of the member used to implement serialization and deserialization.
    /// This is used to implement serialization and deserialization for a wrapper type.
    /// </summary>
    public string? ThroughMember { get; init; }

    /// <summary>
    /// If non-null, the name of the type used to implement serialization and deserialization.
    /// This is used to implement serialization and deserialization for a wrapper type.
    /// </summary>
    public Type? ThroughType { get; init; }
}

[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Struct,
    AllowMultiple = false,
    Inherited = false)]
[Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
#if !SRCGEN
public
#else
internal
#endif
sealed class SerdeWrapAttribute : Attribute
{
    public SerdeWrapAttribute(Type wrapper)
    {
        Wrapper = wrapper;
    }
    public Type Wrapper { get; }
}

#pragma warning restore CS1574

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
    /// Throw an exception during deserialization if any members not expected by the current
    /// type are present.
    /// </summary>
    public bool DenyUnknownMembers { get; init; } = false;

    /// <summary>
    /// Override the formatting for members.
    /// </summary>
    public MemberFormat MemberFormat { get; init; } = MemberFormat.CamelCase;

    /// <summary>
    /// Pick the constructor used for deserialization. Expects a tuple with the same types as
    /// the desired parameter list of the desired constructor.
    /// </summary>
    public Type? ConstructorSignature { get; init; }

    /// <summary>
    /// The default behavior for null is to skip serialization. Set this to true to force
    /// serialization.
    /// </summary>
    public bool SerializeNull { get; init; } = false;
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
    /// Type to use as the wrapper for the ISerialize implementation.
    /// </summary>
    public Type? WrapperSerialize { get; init; } = null;

    /// <summary>
    /// Type to use as the wrapper for the IDeserialize implementation.
    /// </summary>
    public Type? WrapperDeserialize { get; init; } = null;
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