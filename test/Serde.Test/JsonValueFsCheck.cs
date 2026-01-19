using System.Collections.Immutable;
using FsCheck;
using FsCheck.Fluent;
using Serde.Json;
using Xunit;
using static Serde.Json.JsonValue;
using static Serde.Test.JsonFsCheck;

namespace Serde.Test
{
    public class JsonValueFsCheck
    {
        [Fact]
        public void GenTypes()
        {
            var items = Gen.Sample(Gen.Sized(JsonValueGenerators.GenJsonValue), 10, 500);
            foreach (var item in items)
            {
                var serdeStr = JsonSerializer.Serialize(item);
                var de = JsonSerializer.Deserialize<JsonValue>(serdeStr);
                Assert.Equal(item, de);
            }
        }
    }

    internal static class JsonValueGenerators
    {
        public static Gen<JsonValue> GenPrimitive { get; } = Gen.OneOf<JsonValue>(new[] {
            ArbMap.Default.GeneratorFor<int>().Select(a => (JsonValue)new Number(a)),
            ArbMap.Default.GeneratorFor<double>().Where(d => double.IsFinite(d) && !double.IsNaN(d)).Select(a => (JsonValue)new Number(a)),
        });

        public static Gen<JsonValue> GenJsonValue(int size)
        {
            if (size == 0)
            {
                return GenPrimitive;
            }
            else
            {
                return Gen.OneOf<JsonValue>(new[] {
                    GenPrimitive,
                    GenArray(size),
                    GenObject(size)
                });
            }
        }

        public static Gen<JsonValue> GenArray(int size)
        {
            return Gen.Choose(1, 3)
                .SelectMany(arraySize =>
                    TestTypeGenerators.ImmArrayOf<JsonValue>(arraySize, GenJsonValue(size / 2))
                    .Select(values => (JsonValue)new Array(values)));
        }

        public static Gen<JsonValue> GenObject(int size)
        {
            return Gen.Choose(1, 3)
                .SelectMany(arraySize =>
                    Gen.ArrayOf(GenJsonValue(size / 2), arraySize)
                    .Select(values =>
                    {
                        var builder = ImmutableDictionary.CreateBuilder<string, JsonValue>();
                        int index = 0;
                        foreach (var v in values)
                        {
                            builder.Add("item" + index++, v);
                        }
                        return (JsonValue)new Object(builder.ToImmutable());
                    }));
        }
    }
}