
using System;

namespace Serde;

/// <summary>
/// Thrown from implementations of <see cref="IDeserializer" />. Indicates that an unexpected
/// value was seen in the input which cannot be converted to the target type.
/// </summary>
public class DeserializeException(string msg) : Exception(msg)
{
    public static DeserializeException UnassignedMember() => throw new DeserializeException("Not all members were assigned.");

    public static DeserializeException UnknownMember(string name, ISerdeInfo info)
        => new DeserializeException($"Could not find member named '{name ?? "<null>"}' in type '{info.Name}'.");

    public static DeserializeException WrongItemCount(int expected, int actual)
        => new DeserializeException($"Expected {expected} items, got {actual}.");

    public static DeserializeException ExpectedEndOfType(int index)
        => new DeserializeException($"Expected end of type, got index '{index}'.");
}