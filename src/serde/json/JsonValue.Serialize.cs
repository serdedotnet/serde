
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
                    serializer.WriteDouble(v);
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
                        foreach (var (name, node) in members.OrderBy(kvp => kvp.Key))
                        {
                            dict.WriteElement(name, StringProxy.Instance);
                            dict.WriteElement(node, Instance);
                        }
                        dict.End(serdeInfo);
                        break;
                    }
                case JsonValue.Array(ImmutableArray<JsonValue> elements):
                    {
                        var serdeInfo = JsonValue.UnionInfo.ArrayInfo;
                        var enumerable = serializer.WriteCollection(serdeInfo, elements.Length);
                        foreach (var element in elements)
                        {
                            enumerable.WriteElement(element, Instance);
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