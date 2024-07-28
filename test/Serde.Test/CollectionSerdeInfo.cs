
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Serde.Test;

internal sealed record CollectionSerdeInfo(
    string TypeName,
    ISerdeInfo.TypeKind Kind) : ISerdeInfo
{
    public int FieldCount => 0;

    public IList<CustomAttributeData> GetCustomAttributeData(int index)
        => throw GetAOOR(index);

    public ReadOnlySpan<byte> GetSerializeName(int index)
        => throw GetAOOR(index);

    public string GetStringSerializeName(int index)
        => throw GetAOOR(index);

    public int TryGetIndex(ReadOnlySpan<byte> fieldName) => IDeserializeType.IndexNotFound;

    private ArgumentOutOfRangeException GetAOOR(int index)
        => new ArgumentOutOfRangeException(nameof(index), index, $"{TypeName} has no fields or properties.");
}