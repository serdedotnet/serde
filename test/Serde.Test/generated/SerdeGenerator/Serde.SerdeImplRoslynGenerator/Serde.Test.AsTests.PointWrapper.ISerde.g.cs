
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class AsTests
{
    partial struct PointWrapper
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.AsTests.PointWrapper>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = global::Serde.SerdeInfoExtensions.WithName(global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.AsTests.Point, Serde.Test.AsTests.Point>(), "PointWrapper");
            void global::Serde.ISerialize<Serde.Test.AsTests.PointWrapper>.Serialize(Serde.Test.AsTests.PointWrapper value, global::Serde.ISerializer serializer)
            {
                global::Serde.SerializeProvider.GetSerialize<Serde.Test.AsTests.Point, Serde.Test.AsTests.Point>().Serialize((Serde.Test.AsTests.Point)value, serializer);
            }

            Serde.Test.AsTests.PointWrapper global::Serde.IDeserialize<Serde.Test.AsTests.PointWrapper>.Deserialize(global::Serde.IDeserializer deserializer)
            {
                return (Serde.Test.AsTests.PointWrapper)global::Serde.DeserializeProvider.GetDeserialize<Serde.Test.AsTests.Point, Serde.Test.AsTests.Point>().Deserialize(deserializer);
            }

        }
    }
}
