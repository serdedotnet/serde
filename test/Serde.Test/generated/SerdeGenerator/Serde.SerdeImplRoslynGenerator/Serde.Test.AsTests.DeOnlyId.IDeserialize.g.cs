
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class AsTests
{
    partial record struct DeOnlyId
    {
        sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.AsTests.DeOnlyId>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = global::Serde.SerdeInfoExtensions.WithName(global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), "DeOnlyId");
            Serde.Test.AsTests.DeOnlyId global::Serde.IDeserialize<Serde.Test.AsTests.DeOnlyId>.Deserialize(global::Serde.IDeserializer deserializer)
            {
                return (Serde.Test.AsTests.DeOnlyId)global::Serde.DeserializeProvider.GetDeserialize<string, global::Serde.StringProxy>().Deserialize(deserializer);
            }

        }
    }
}
