
using System;
using Microsoft.CodeAnalysis;

namespace Serde;

internal readonly record struct TypeOptions()
{
    public bool DenyUnknownMembers { get; init; } = false;
    public MemberFormat MemberFormat { get; init; } = MemberFormat.CamelCase;
    public ITypeSymbol? ConstructorSignature { get; init; } = null;
    public bool SerializeNull { get; init; } = false;
}

internal readonly record struct MemberOptions()
{
    /// <see cref="SerdeMemberOptions.ThrowIfMissing" />
    public bool ThrowIfMissing { get; init; } = false;

    /// <see cref="SerdeMemberOptions.Rename" />
    public string? Rename { get; init; } = null;

    /// <see cref="SerdeMemberOptions.ProvideAttributes" />
    public bool ProvideAttributes { get; init; } = false;

    /// <see cref="SerdeMemberOptions.ProvideAttributes" />
    public bool? SerializeNull { get; init; } = null;
}