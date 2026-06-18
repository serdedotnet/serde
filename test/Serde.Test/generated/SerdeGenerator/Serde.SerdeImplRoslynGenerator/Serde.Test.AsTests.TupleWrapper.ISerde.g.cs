
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class AsTests
{
    partial record struct TupleWrapper
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.AsTests.TupleWrapper>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = global::Serde.SerdeInfoExtensions.WithName(global::Serde.SerdeInfoProvider.GetSerializeInfo<(int, string), Serde.TupleProxy.Ser<int, string, global::Serde.I32Proxy, global::Serde.StringProxy>>(), "TupleWrapper");
            void global::Serde.ISerialize<Serde.Test.AsTests.TupleWrapper>.Serialize(Serde.Test.AsTests.TupleWrapper value, global::Serde.ISerializer serializer)
            {
                value.Deconstruct(out var __e0, out var __e1);
                global::Serde.SerializeProvider.GetSerialize<(int, string), Serde.TupleProxy.Ser<int, string, global::Serde.I32Proxy, global::Serde.StringProxy>>().Serialize((__e0, __e1), serializer);
            }

            Serde.Test.AsTests.TupleWrapper global::Serde.IDeserialize<Serde.Test.AsTests.TupleWrapper>.Deserialize(global::Serde.IDeserializer deserializer)
            {
                var __t = global::Serde.DeserializeProvider.GetDeserialize<(int, string), Serde.TupleProxy.De<int, string, global::Serde.I32Proxy, global::Serde.StringProxy>>().Deserialize(deserializer);
                return new Serde.Test.AsTests.TupleWrapper(__t.Item1, __t.Item2);
            }

        }
    }
}
