
using System;
using Microsoft.CodeAnalysis;

namespace Serde;

internal record TypeOptions
{
    public bool DenyUnknownMembers { get; init; } = false;
    public MemberFormat MemberFormat { get; init; } = MemberFormat.CamelCase;
    public ITypeSymbol? ConstructorSignature { get; init; } = null;
    public bool SerializeNull { get; init; } = false;
}

internal readonly record struct MemberOptions()
{
    /// <see cref="SerdeMemberOptions.ThrowIfMissing" />
    public bool? ThrowIfMissing { get; init; } = null;

    /// <see cref="SerdeMemberOptions.Rename" />
    public string? Rename { get; init; } = null;

    /// <see cref="SerdeMemberOptions.ProvideAttributes" />
    public bool ProvideAttributes { get; init; } = false;

    /// <see cref="SerdeMemberOptions.ProvideAttributes" />
    public bool? SerializeNull { get; init; } = null;

    /// <see cref="SerdeMemberOptions.Skip" />
    public bool Skip { get; init; } = false;

    /// <see cref="SerdeMemberOptions.SkipSerialize" />
    public bool SkipSerialize { get; init; } = false;

    /// <see cref="SerdeMemberOptions.SkipDeserialize" />
    public bool SkipDeserialize { get; init; } = false;
}