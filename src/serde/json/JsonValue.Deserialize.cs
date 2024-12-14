
using System;
using System.Collections.Immutable;
using System.Text;

namespace Serde.Json
{
    partial record JsonValue : IDeserializeProvider<JsonValue>
    {
        static ISerdeInfo ISerdeInfoProvider.SerdeInfo => JsonValue.UnionInfo.Instance;

        static IDeserialize<JsonValue> IDeserializeProvider<JsonValue>.DeserializeInstance { get; }
            = JsonValueDeserialize.Instance;
    }

    file sealed class JsonValueDeserialize : IDeserialize<JsonValue>
    {
        public static JsonValueDeserialize Instance { get; } = new();

        private JsonValueDeserialize() { }

        JsonValue IDeserialize<JsonValue>.Deserialize(IDeserializer deserializer)
        {
            return deserializer.ReadAny(Visitor.Instance);
        }

        private sealed class Visitor : IDeserializeVisitor<JsonValue>
        {
            public static readonly Visitor Instance = new Visitor();
            private Visitor() { }

            public string ExpectedTypeName => nameof(JsonValue);

            public JsonValue VisitEnumerable<D>(ref D d)
                where D : IDeserializeEnumerable
            {
                var builder = ImmutableArray.CreateBuilder<JsonValue>(d.SizeOpt ?? 3);
                var deserialize = JsonValueDeserialize.Instance;
                while (d.TryGetNext<JsonValue, JsonValueDeserialize>(deserialize, out var next))
                {
                    builder.Add(next);
                }
                return new JsonValue.Array(builder.ToImmutable());
            }

            public JsonValue VisitDictionary<D>(ref D d)
                where D : IDeserializeDictionary
            {
                var builder = ImmutableDictionary.CreateBuilder<string, JsonValue>();
                var deserialize = JsonValueDeserialize.Instance;
                while (d.TryGetNextEntry<string, JsonValue, StringProxy, JsonValueDeserialize>(StringProxy.Instance, deserialize, out var next))
                {
                    builder.Add(next.Item1, next.Item2);
                }
                return new JsonValue.Object(builder.ToImmutable());
            }

            public JsonValue VisitBool(bool b) => new JsonValue.Bool(b);
            public JsonValue VisitDouble(double d) => new JsonValue.Number(d);
            public JsonValue VisitString(string s) => new JsonValue.String(s);
            public JsonValue VisitUtf8Span(ReadOnlySpan<byte> s) => VisitString(Encoding.UTF8.GetString(s));
        }
    }
}