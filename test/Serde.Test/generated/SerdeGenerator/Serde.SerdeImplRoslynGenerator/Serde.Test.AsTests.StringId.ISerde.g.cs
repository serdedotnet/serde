
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class AsTests
{
    partial struct StringId
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.AsTests.StringId>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = global::Serde.SerdeInfoExtensions.WithName(global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), "StringId");
            void global::Serde.ISerialize<Serde.Test.AsTests.StringId>.Serialize(Serde.Test.AsTests.StringId value, global::Serde.ISerializer serializer)
            {
                global::Serde.SerializeProvider.GetSerialize<string, global::Serde.StringProxy>().Serialize((string)value, serializer);
            }

            Serde.Test.AsTests.StringId global::Serde.IDeserialize<Serde.Test.AsTests.StringId>.Deserialize(global::Serde.IDeserializer deserializer)
            {
                return (Serde.Test.AsTests.StringId)global::Serde.DeserializeProvider.GetDeserialize<string, global::Serde.StringProxy>().Deserialize(deserializer);
            }

        }
    }
}
