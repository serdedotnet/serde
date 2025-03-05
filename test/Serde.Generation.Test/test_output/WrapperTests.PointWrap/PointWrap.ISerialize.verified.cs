//HintName: PointWrap.ISerialize.cs

#nullable enable

using System;
using Serde;
partial struct PointWrap : Serde.ISerializeProvider<Point>
{
    static ISerialize<Point> ISerializeProvider<Point>.SerializeInstance
        => PointWrapSerializeProxy.Instance;

    sealed partial class PointWrapSerializeProxy :Serde.ISerialize<Point>
    {
        void global::Serde.ISerialize<Point>.Serialize(Point value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<PointWrap>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteI32(_l_info, 0, value.X);
            _l_type.WriteI32(_l_info, 1, value.Y);
            _l_type.End(_l_info);
        }
        public static readonly PointWrapSerializeProxy Instance = new();
        private PointWrapSerializeProxy() { }

    }
}
