
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class AsTests
{
    partial class PriorityProxy : Serde.ISerde<Serde.Test.AsTests.Priority>
    {
        void global::Serde.ISerialize<Serde.Test.AsTests.Priority>.Serialize(Serde.Test.AsTests.Priority value, global::Serde.ISerializer serializer)
        {
            serializer.WriteI32((int)value);

        }
        Serde.Test.AsTests.Priority IDeserialize<Serde.Test.AsTests.Priority>.Deserialize(IDeserializer deserializer)
        {
            return (Serde.Test.AsTests.Priority)deserializer.ReadI32();
        }
    }
}
