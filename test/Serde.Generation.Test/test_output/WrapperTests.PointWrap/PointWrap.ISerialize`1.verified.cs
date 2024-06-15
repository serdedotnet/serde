//HintName: PointWrap.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct PointWrap : Serde.ISerialize<Point>
{
    void ISerialize<Point>.Serialize(Point value, ISerializer serializer)
    {
        var _l_typeInfo = PointSerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeField<int, Int32Wrap>(_l_typeInfo, 0, _point.X);
        type.SerializeField<int, Int32Wrap>(_l_typeInfo, 1, _point.Y);
        type.End();
    }
}