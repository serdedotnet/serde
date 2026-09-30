using System.Collections.Immutable;
using System.Linq;

namespace Serde;

/// <summary>
/// Thrown when a type is deserialized without all of its required members being present.
/// </summary>
public sealed class UnassignedMemberException : DeserializeException
{
    internal UnassignedMemberException(
        ISerdeInfo serdeInfo,
        ImmutableArray<int> missingFieldIndices
    )
        : base(FormatMessage(serdeInfo, missingFieldIndices))
    {
        SerdeInfo = serdeInfo;
        MissingFieldIndices = missingFieldIndices;
    }

    /// <summary>
    /// The type that was missing required members.
    /// </summary>
    public ISerdeInfo SerdeInfo { get; }

    /// <summary>
    /// The field indices, in <see cref="SerdeInfo" />, of the required members that were not assigned.
    /// </summary>
    public ImmutableArray<int> MissingFieldIndices { get; }

    private static string FormatMessage(ISerdeInfo info, ImmutableArray<int> missing)
    {
        var names = string.Join(", ", missing.Select(i => $"'{info.GetFieldStringName(i)}'"));
        var noun = missing.Length == 1 ? "member" : "members";
        return $"Missing required {noun} {names} in type '{info.Name}'.";
    }
}
