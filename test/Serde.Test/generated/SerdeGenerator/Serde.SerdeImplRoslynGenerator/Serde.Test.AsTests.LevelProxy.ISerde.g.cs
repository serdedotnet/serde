
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class AsTests
{
    partial class LevelProxy : Serde.ISerde<Serde.Test.AsTests.Level>
    {
        void global::Serde.ISerialize<Serde.Test.AsTests.Level>.Serialize(Serde.Test.AsTests.Level value, global::Serde.ISerializer serializer)
        {
            serializer.WriteU8((byte)value);

        }
        Serde.Test.AsTests.Level IDeserialize<Serde.Test.AsTests.Level>.Deserialize(IDeserializer deserializer)
        {
            return (Serde.Test.AsTests.Level)deserializer.ReadU8();
        }
    }
}
