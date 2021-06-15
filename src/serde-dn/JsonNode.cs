
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Serde.Test")]

namespace Serde.Test
{
    internal abstract record JsonNode : ISerialize
    {
        private protected JsonNode() { }

        public abstract void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
            where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable>
            where TSerializeType : ISerializeType
            where TSerializeEnumerable : ISerializeEnumerable;

        public static implicit operator JsonNode(int i) => new JsonNumber(i);
        public static implicit operator JsonNode(bool b) => new JsonBool(b);
        public static implicit operator JsonNode(string s) => new JsonString(s);
    }
    internal record JsonNumber : JsonNode
    {
        private readonly double _d;
        public JsonNumber(int i) { _d = i; }
        public JsonNumber(long l) { _d = l; }
        public JsonNumber(double d) { _d = d; }

        public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
        {
            serializer.Serialize(_d);
        }
    }
    internal record JsonBool(bool Value) : JsonNode
    {
        public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
        {
            serializer.Serialize(Value);
        }
    }
    internal record JsonString(string Value) : JsonNode
    {
        public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
        {
            serializer.Serialize(Value);
        }
    }
    internal record JsonObject(ImmutableArray<(string FieldName, JsonNode Node)> Members) : JsonNode
    {
        public JsonObject(IEnumerable<(string FieldName, JsonNode Value)> members)
            : this(members.ToImmutableArray())
        { }

        public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
        {
            var type = serializer.SerializeType("", Members.Length);
            foreach (var (name, node) in Members)
            {
                type.SerializeField(name, node);
            }
            type.End();
        }
    }
    internal record JsonArray(ImmutableArray<JsonNode> Elements) : JsonNode
    {
        public JsonArray(IEnumerable<JsonNode> elements)
            : this(elements.ToImmutableArray())
        { }

        public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
        {
            var enumerable = serializer.SerializeEnumerable(Elements.Length);
            foreach (var element in Elements)
            {
                enumerable.SerializeElement(element);
            }
            enumerable.End();
        }
    }
}