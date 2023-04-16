//HintName: PointWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct PointWrap : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("Point", 2);
        type.SerializeField("x", new Int32Wrap(_point.X));
        type.SerializeField("y", new Int32Wrap(_point.Y));
        type.End();
    }
}