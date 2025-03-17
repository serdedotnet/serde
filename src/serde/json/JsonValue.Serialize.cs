
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Serde.Json
{
    partial record JsonValue : ISerializeProvider<JsonValue>
    {
        static ISerialize<JsonValue> ISerializeProvider<JsonValue>.SerializeInstance { get; }
            = JsonValueSerialize.Instance;
    }

    file sealed class JsonValueSerialize : ISerialize<JsonValue>
    {
        public static JsonValueSerialize Instance { get; } = new();
        public ISerdeInfo SerdeInfo { get; } = JsonValue.UnionInfo.Instance;

        private JsonValueSerialize() { }

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
                        var dict = serializer.WriteType(serdeInfo);
                        int index = 0;
                        foreach (var (name, node) in members.OrderBy(kvp => kvp.Key))
                        {
                            dict.WriteString(serdeInfo, index++, name);
                            dict.WriteValue(serdeInfo, index++, node, Instance);
                        }
                        dict.End(serdeInfo);
                        break;
                    }
                case JsonValue.Array(ImmutableArray<JsonValue> elements):
                    {
                        var serdeInfo = JsonValue.UnionInfo.ArrayInfo;
                        var enumerable = serializer.WriteType(serdeInfo);
                        int index = 0;
                        foreach (var element in elements)
                        {
                            enumerable.WriteValue(serdeInfo, index++, element, Instance);
                        }
                        enumerable.End(serdeInfo);
                        break;
                    }
                case JsonValue.Null n:
                    serializer.WriteNull();
                    break;
                default:
                    throw new InvalidOperationException($"Unknown JsonValue type: {value.GetType()}");
            }
        }
    }
}