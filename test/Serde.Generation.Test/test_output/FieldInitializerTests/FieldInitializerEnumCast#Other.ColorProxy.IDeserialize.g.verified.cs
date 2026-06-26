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
        var de = deserializer.ReadType(serdeInfo);
        var (index, errorName) = de.TryReadIndexWithName(serdeInfo);
        if (index == ITypeDeserializer.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }
        Other.Color _l_result;
        if (index == ITypeDeserializer.EndOfType)
        {
            // Assume we want to read the underlying value
            _l_result = (Other.Color)de.ReadI32(serdeInfo, index);
        }
        else
        {
            _l_result = index switch {
                0 => Other.Color.Red,
                1 => Other.Color.Green,
                _ => throw new InvalidOperationException($"Unexpected index: {index}")
            };
        }
        de.End(serdeInfo);
        return _l_result;
    }
}
