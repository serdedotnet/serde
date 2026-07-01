//HintName: ColorProxy.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial class ColorProxy : Serde.ISerde<Color>
{
    void global::Serde.ISerialize<Color>.Serialize(Color value, global::Serde.ISerializer serializer)
    {
        serializer.WriteI32((int)value);

    }
    Color IDeserialize<Color>.Deserialize(IDeserializer deserializer)
    {
        return (Color)deserializer.ReadI32();
    }
}
