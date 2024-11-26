//HintName: PointWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct PointWrap : Serde.ISerializeProvider<Point>
{
    static ISerialize<Point> ISerializeProvider<Point>.SerializeInstance => PointWrapSerializeProxy.Instance;

    sealed class PointWrapSerializeProxy : Serde.ISerialize<Point>
    {
        void ISerialize<Point>.Serialize(Point value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<PointWrap>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<int, global::Serde.Int32Proxy>(_l_serdeInfo, 0, value.X);
            type.SerializeField<int, global::Serde.Int32Proxy>(_l_serdeInfo, 1, value.Y);
            type.End();
        }

        public static readonly PointWrapSerializeProxy Instance = new();
        private PointWrapSerializeProxy()
        {
        }
    }
}