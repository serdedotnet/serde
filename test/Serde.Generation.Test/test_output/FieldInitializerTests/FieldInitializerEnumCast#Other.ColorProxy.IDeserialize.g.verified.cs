//HintName: Other.ColorProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;

namespace Other;

partial class ColorProxy : Serde.IDeserialize<Other.Color>
{
    Other.Color IDeserialize<Other.Color>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        using var de = deserializer.ReadType(serdeInfo);
        var (index, errorName) = de.TryReadIndexWithName(serdeInfo);
        if (index == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        if (index == ITypeDeserializer.EndOfType)
        {
            // Assume we want to read the underlying value
            return (Other.Color)de.ReadI32(serdeInfo, index);
        }
        return index switch {
            0 => Other.Color.Red,
            1 => Other.Color.Green,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
}
