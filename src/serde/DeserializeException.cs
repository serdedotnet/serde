using System;
using System.Collections.Immutable;

namespace Serde;

/// <summary>
/// Thrown from implementations of <see cref="IDeserializer" />. Indicates that an unexpected
/// value was seen in the input which cannot be converted to the target type.
/// </summary>
public class DeserializeException(string msg) : Exception(msg)
{
    public static DeserializeException UnassignedMember() =>
        throw new DeserializeException("Not all members were assigned.");

    /// <summary>
    /// Creates an exception for a type whose required members were not all assigned.
    /// <paramref name="assignedMask" /> and <paramref name="requiredMask" /> are bitfields
    /// indexed by field index in <paramref name="info" />.
    /// </summary>
    public static UnassignedMemberException UnassignedMember(
        ulong assignedMask,
        ulong requiredMask,
        ISerdeInfo info
    )
    {
        var missing = ImmutableArray.CreateBuilder<int>();
        var missingMask = requiredMask & ~assignedMask;
        for (int i = 0; i < 64; i++)
        {
            if ((missingMask & (1UL << i)) != 0)
            {
                missing.Add(i);
            }
        }
        return new UnassignedMemberException(info, missing.ToImmutable());
    }

    public static DeserializeException UnknownMember(string name, ISerdeInfo info) =>
        new DeserializeException(
            $"Could not find member named '{name ?? "<null>"}' in type '{info.Name}'."
        );

    public static DeserializeException WrongItemCount(int expected, int actual) =>
        new DeserializeException($"Expected {expected} items, got {actual}.");

    public static DeserializeException ExpectedEndOfType(int index) =>
        new DeserializeException($"Expected end of type, got index '{index}'.");

    public static DeserializeException DuplicateKey(string propertyName, ISerdeInfo info) =>
        new DeserializeException($"Duplicate key '{propertyName}' in type '{info.Name}'.");

    /// <summary>
    /// Checks if a field has already been assigned and throws a DuplicateKey exception if it has.
    /// </summary>
    public static void ThrowIfDuplicate(byte assignedMask, int fieldIndex, ISerdeInfo serdeInfo)
    {
        if ((assignedMask & ((byte)1 << fieldIndex)) != 0)
        {
            throw DuplicateKey(serdeInfo.GetFieldStringName(fieldIndex), serdeInfo);
        }
    }

    /// <summary>
    /// Checks if a field has already been assigned and throws a DuplicateKey exception if it has.
    /// </summary>
    public static void ThrowIfDuplicate(ushort assignedMask, int fieldIndex, ISerdeInfo serdeInfo)
    {
        if ((assignedMask & ((ushort)1 << fieldIndex)) != 0)
        {
            throw DuplicateKey(serdeInfo.GetFieldStringName(fieldIndex), serdeInfo);
        }
    }

    /// <summary>
    /// Checks if a field has already been assigned and throws a DuplicateKey exception if it has.
    /// </summary>
    public static void ThrowIfDuplicate(uint assignedMask, int fieldIndex, ISerdeInfo serdeInfo)
    {
        if ((assignedMask & ((uint)1 << fieldIndex)) != 0)
        {
            throw DuplicateKey(serdeInfo.GetFieldStringName(fieldIndex), serdeInfo);
        }
    }

    /// <summary>
    /// Checks if a field has already been assigned and throws a DuplicateKey exception if it has.
    /// </summary>
    public static void ThrowIfDuplicate(ulong assignedMask, int fieldIndex, ISerdeInfo serdeInfo)
    {
        if ((assignedMask & ((ulong)1 << fieldIndex)) != 0)
        {
            throw DuplicateKey(serdeInfo.GetFieldStringName(fieldIndex), serdeInfo);
        }
    }
}
