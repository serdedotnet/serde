//HintName: PointWrapper.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial struct PointWrapper
{
    sealed partial class _SerdeObj : global::Serde.ISerde<PointWrapper>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = global::Serde.SerdeInfoExtensions.WithName(global::Serde.SerdeInfoProvider.GetSerializeInfo<Point, Point>(), "PointWrapper");
        void global::Serde.ISerialize<PointWrapper>.Serialize(PointWrapper value, global::Serde.ISerializer serializer)
        {
            global::Serde.SerializeProvider.GetSerialize<Point, Point>().Serialize((Point)value, serializer);
        }

        PointWrapper global::Serde.IDeserialize<PointWrapper>.Deserialize(global::Serde.IDeserializer deserializer)
        {
            return (PointWrapper)global::Serde.DeserializeProvider.GetDeserialize<Point, Point>().Deserialize(deserializer);
        }

    }
}
