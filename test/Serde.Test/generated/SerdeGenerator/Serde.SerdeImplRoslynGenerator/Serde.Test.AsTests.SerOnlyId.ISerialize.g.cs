
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class AsTests
{
    partial record struct SerOnlyId
    {
        sealed partial class _SerObj : Serde.ISerialize<Serde.Test.AsTests.SerOnlyId>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = global::Serde.SerdeInfoExtensions.WithName(global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), "SerOnlyId");
            void global::Serde.ISerialize<Serde.Test.AsTests.SerOnlyId>.Serialize(Serde.Test.AsTests.SerOnlyId value, global::Serde.ISerializer serializer)
            {
                global::Serde.SerializeProvider.GetSerialize<string, global::Serde.StringProxy>().Serialize((string)value, serializer);
            }

        }
    }
}
