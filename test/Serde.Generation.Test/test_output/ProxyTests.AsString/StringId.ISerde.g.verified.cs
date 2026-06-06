//HintName: StringId.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial struct StringId
{
    sealed partial class _SerdeObj : global::Serde.ISerde<StringId>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = global::Serde.SerdeInfoExtensions.WithName(global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), "StringId");
        void global::Serde.ISerialize<StringId>.Serialize(StringId value, global::Serde.ISerializer serializer)
        {
            global::Serde.SerializeProvider.GetSerialize<string, global::Serde.StringProxy>().Serialize((string)value, serializer);
        }

        StringId global::Serde.IDeserialize<StringId>.Deserialize(global::Serde.IDeserializer deserializer)
        {
            return (StringId)global::Serde.DeserializeProvider.GetDeserialize<string, global::Serde.StringProxy>().Deserialize(deserializer);
        }

    }
}
