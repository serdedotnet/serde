//HintName: PointWrap.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct PointWrap : Serde.ISerialize<Point>
{
    void ISerialize<Point>.Serialize(Point value, ISerializer serializer)
    {
        var type = serializer.SerializeType("Point", 2);
        type.SerializeField<int, Int32Wrap>("x", _point.X);
        type.SerializeField<int, Int32Wrap>("y", _point.Y);
        type.End();
    }
}