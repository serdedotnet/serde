
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace Serde.Json
{
    partial record JsonValue : IDeserialize<JsonValue>
    {
        static ValueTask<JsonValue> IDeserialize<JsonValue>.Deserialize<D>(D deserializer)
        {
            return deserializer.DeserializeAny<JsonValue, Visitor>(Visitor.Instance);
        }

        private sealed class Visitor : IDeserializeVisitor<JsonValue>
        {
            public static readonly Visitor Instance = new Visitor();
            private Visitor() { }

            public string ExpectedTypeName => nameof(JsonValue);

            public async ValueTask<JsonValue> VisitEnumerable<D>(D d)
                where D : IDeserializeEnumerable
            {
                var builder = ImmutableArray.CreateBuilder<JsonValue>(d.SizeOpt ?? 3);
                while (await d.TryGetNext<JsonValue, JsonValue>() is var nextOpt && nextOpt.HasValue)
                {
                    builder.Add(nextOpt.GetValueOrDefault());
                }
                return new Array(builder.ToImmutable());
            }

            public async ValueTask<JsonValue> VisitDictionary<D>(D d)
                where D : IDeserializeDictionary
            {
                var builder = ImmutableDictionary.CreateBuilder<string, JsonValue>();
                while (await d.TryGetNextEntry<string, StringWrap, JsonValue, JsonValue>() is var entryOpt && entryOpt.HasValue)
                {
                    var (k, v) = entryOpt.GetValueOrDefault();
                    builder.Add(k, v);
                }
                return new Object(builder.ToImmutable());
            }

            public JsonValue VisitBool(bool b) => new Bool(b);
            public JsonValue VisitI64(long i) => new Number(i);
            public JsonValue VisitString(string s) => new String(s);
        }
    }
}