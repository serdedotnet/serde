
using System;

namespace Serde;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class SerdeOptions : Attribute
{
    public bool DenyUnknownMembers { get; init; } = false;
    /// <summary>
    /// Override the formatting for members.
    /// </summary>
    public MemberFormat MemberFormat { get; init; } = MemberFormat.None;
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