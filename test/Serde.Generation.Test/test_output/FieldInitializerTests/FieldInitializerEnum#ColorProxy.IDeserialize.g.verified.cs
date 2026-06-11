//HintName: ColorProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class ColorProxy : Serde.IDeserialize<Color>
{
    Color IDeserialize<Color>.Deserialize(IDeserializer deserializer)
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
            return (Color)de.ReadI32(serdeInfo, index);
        }
        return index switch {
            0 => Color.Red,
            1 => Color.Green,
            2 => Color.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
    }
}
