//HintName: SerOnlyId.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial struct SerOnlyId
{
    sealed partial class _SerObj : Serde.ISerialize<SerOnlyId>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = global::Serde.SerdeInfoExtensions.WithName(global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), "SerOnlyId");
        void global::Serde.ISerialize<SerOnlyId>.Serialize(SerOnlyId value, global::Serde.ISerializer serializer)
        {
            global::Serde.SerializeProvider.GetSerialize<int, global::Serde.I32Proxy>().Serialize((int)value, serializer);
        }

    }
}
