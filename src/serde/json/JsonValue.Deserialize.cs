
using System;
using System.Collections.Immutable;
using System.Text;

namespace Serde.Json
{
    partial record JsonValue : IDeserialize<JsonValue>
    {
        static JsonValue IDeserialize<JsonValue>.Deserialize(IDeserializer deserializer)
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
                while (d.TryGetNext<JsonValue, JsonValue>(out var next))
                {
                    builder.Add(next);
                }
                return new Array(builder.ToImmutable());
            }

            public JsonValue VisitDictionary<D>(ref D d)
                where D : IDeserializeDictionary
            {
                var builder = ImmutableDictionary.CreateBuilder<string, JsonValue>();
                while (d.TryGetNextEntry<string, StringWrap, JsonValue, JsonValue>(out var next))
                {
                    builder.Add(next.Item1, next.Item2);
                }
                return new Object(builder.ToImmutable());
            }

            public JsonValue VisitBool(bool b) => new Bool(b);
            public JsonValue VisitDouble(double d) => new Number(d);
            public JsonValue VisitString(string s) => new String(s);
            public JsonValue VisitUtf8Span(ReadOnlySpan<byte> s) => VisitString(Encoding.UTF8.GetString(s));
        }
    }
}