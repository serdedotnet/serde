using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Serde.Json
{
    partial record JsonValue : ISerializeProvider<JsonValue>, IDeserializeProvider<JsonValue>
    {
        static IDeserialize<JsonValue> IDeserializeProvider<JsonValue>.Instance { get; } =
            new JsonValueDeserialize();

        static ISerialize<JsonValue> ISerializeProvider<JsonValue>.Instance { get; } =
            new JsonValueSerialize();
    }

    file sealed class JsonValueDeserialize : IDeserialize<JsonValue>
    {
        public ISerdeInfo SerdeInfo => JsonValue.UnionInfo.Instance;

        public JsonValue Deserialize(IDeserializer deserializer)
        {
            if (deserializer is not BaseJsonDeserializer jsonDeserializer)
            {
                throw new ArgumentException(
                    "deserializer must be JsonDeserializer",
                    nameof(deserializer)
                );
            }
            return jsonDeserializer.ReadJsonValue();
        }
    }

    file sealed class JsonValueSerialize : ISerialize<JsonValue>
    {
        public ISerdeInfo SerdeInfo => JsonValue.UnionInfo.Instance;

#if NET11_0_OR_GREATER
        async Task ISerialize<JsonValue>.Serialize(JsonValue value, ISerializer serializer)
        {
            switch (value)
            {
                case JsonValue.Number(double number):
                    await serializer.WriteF64(number);
                    break;
                case JsonValue.Bool(bool boolean):
                    await serializer.WriteBool(boolean);
                    break;
                case JsonValue.String(string text):
                    await serializer.WriteString(text);
                    break;
                case JsonValue.Object(ImmutableDictionary<string, JsonValue> members):
                    {
                        var info = JsonValue.UnionInfo.ObjectInfo;
                        var dictionary = await serializer.WriteCollection(info, members.Count);
                        int index = 0;
                        foreach (var (name, node) in members.OrderBy(kvp => kvp.Key))
                        {
                            await dictionary.WriteString(info, index++, name);
                            await ((ISerialize<JsonValue>)this).SerializeAsField(
                                dictionary, info, index++, node
                            );
                        }
                        await dictionary.End(info);
                        break;
                    }
                case JsonValue.Array(ImmutableArray<JsonValue> elements):
                    {
                        var info = JsonValue.UnionInfo.ArrayInfo;
                        var array = await serializer.WriteCollection(info, elements.Length);
                        for (int index = 0; index < elements.Length; index++)
                        {
                            await ((ISerialize<JsonValue>)this).SerializeAsField(
                                array, info, index, elements[index]
                            );
                        }
                        await array.End(info);
                        break;
                    }
                case JsonValue.Null:
                    await serializer.WriteNull();
                    break;
                default:
                    throw new InvalidOperationException($"Unknown JsonValue type: {value.GetType()}");
            }
        }
#else
        void ISerialize<JsonValue>.Serialize(JsonValue value, ISerializer serializer)
        {
            switch (value)
            {
                case JsonValue.Number(double v):
                    serializer.WriteF64(v);
                    break;
                case JsonValue.Bool(bool v):
                    serializer.WriteBool(v);
                    break;
                case JsonValue.String(string v):
                    serializer.WriteString(v);
                    break;
                case JsonValue.Object(ImmutableDictionary<string, JsonValue> members):
                {
                    var serdeInfo = JsonValue.UnionInfo.ObjectInfo;
                    var dict = serializer.WriteCollection(serdeInfo, members.Count);
                    int index = 0;
                    foreach (var (name, node) in members.OrderBy(kvp => kvp.Key))
                    {
                        dict.WriteString(serdeInfo, index++, name);
                        dict.WriteValue(serdeInfo, index++, node, this);
                    }
                    dict.End(serdeInfo);
                    break;
                }
                case JsonValue.Array(ImmutableArray<JsonValue> elements):
                {
                    var serdeInfo = JsonValue.UnionInfo.ArrayInfo;
                    var enumerable = serializer.WriteCollection(serdeInfo, elements.Length);
                    int index = 0;
                    foreach (var element in elements)
                    {
                        enumerable.WriteValue(serdeInfo, index++, element, this);
                    }
                    enumerable.End(serdeInfo);
                    break;
                }
                case JsonValue.Null n:
                    serializer.WriteNull();
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unknown JsonValue type: {value.GetType()}"
                    );
            }
        }
#endif
    }
}
