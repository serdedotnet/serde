
using System;
using System.Diagnostics;

namespace Serde;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
[Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
public sealed class GenerateSerialize : Attribute
{ }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
[Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
public sealed class GenerateDeserialize : Attribute
{ }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
[Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
public sealed class GenerateSerde : Attribute
{ }

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
[Conditional("EMIT_GENERATE_SERDE_ATTRIBUTE")]
public sealed class GenerateWrapper : Attribute
{
    public GenerateWrapper(string memberName) { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class SerdeTypeOptions : Attribute
{
    public bool DenyUnknownMembers { get; init; } = false;
    /// <summary>
    /// Override the formatting for members.
    /// </summary>
    public MemberFormat MemberFormat { get; init; } = MemberFormat.None;
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class SerdeMemberOptions : Attribute
{
    public bool NullIfMissing { get; init; } = false;
}

public enum MemberFormat : byte
{
    /// <summary>
    /// Use the original name of the member.
    /// </summary>
    None,
    /// <summary>
    /// "PascalCase"
    /// </summary>
    PascalCase,
    /// <summary>
    /// "camelCase"
    /// </summary>
    CamelCase
}