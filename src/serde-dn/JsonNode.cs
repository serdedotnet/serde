
using System.Collections.Generic;

namespace Serde.Test
{
    abstract record JsonNode : ISerialize
    {
        internal JsonNode() { }

        public abstract void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable;

        public static implicit operator JsonNode(int i) => new JsonNumber(i);
    }
    abstract record JsonValue : JsonNode;
    partial record JsonNumber(int Value) : JsonValue;
    partial record JsonObject(IReadOnlyList<(string FieldName, JsonNode Node)> Members) : JsonNode;

    partial record JsonNumber
    {
        public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
        {
            serializer.Serialize(Value);
        }
    }

    partial record JsonObject
    {
        public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
        {
            var type = serializer.SerializeType("", Members.Count);
            foreach (var (name, node) in Members)
            {
                type.SerializeField(name, node);
            }
            type.End();
        }
    }
}