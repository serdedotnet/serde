using System.Collections.Immutable;
using CsCheck;
using Serde.Json;
using Xunit;
using static Serde.Json.JsonValue;

namespace Serde.Test
{
    public class JsonValueFsCheck
    {
        [Fact]
        public void GenTypes()
        {
            JsonValueGenerators.GenJsonValue.Sample(item =>
            {
                var serdeStr = JsonSerializer.Serialize(item);
                var de = JsonSerializer.Deserialize<JsonValue>(serdeStr);
                Assert.Equal(item, de);
            }, iter: 500, threads: 1);
        }
    }

    internal static class JsonValueGenerators
    {
        public static Gen<JsonValue> GenPrimitive { get; } = Gen.OneOf<JsonValue>(
            Gen.Int.Select(a => (JsonValue)new Number(a)),
            Gen.Double.Where(d => double.IsFinite(d) && !double.IsNaN(d)).Select(a => (JsonValue)new Number(a))
        );

        public static Gen<JsonValue> GenJsonValue { get; } = Gen.Recursive<JsonValue>((depth, self) =>
        {
            if (depth >= 4) return GenPrimitive;

            var genArray = Gen.Int[1, 3].SelectMany(n =>
                self.Array[n].Select(values => (JsonValue)new Array(values.ToImmutableArray())));

            var genObject = Gen.Int[1, 3].SelectMany(n =>
                self.Array[n].Select(values =>
                {
                    var builder = ImmutableDictionary.CreateBuilder<string, JsonValue>();
                    int index = 0;
                    foreach (var v in values)
                    {
                        builder.Add("item" + index++, v);
                    }
                    return (JsonValue)new Object(builder.ToImmutable());
                }));

            return Gen.OneOf<JsonValue>(GenPrimitive, genArray, genObject);
        });
    }
}