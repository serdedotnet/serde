
using System;
using Serde.Json;
using Xunit;

namespace Serde.Test
{
    public partial class Int128Tests
    {
        [Fact]
        public void SerializeInt128()
        {
            var value = new Int128Wrap(Int128.MaxValue);
            var json = JsonSerializer.Serialize(value);
            Assert.Equal("""{"value":"170141183460469231731687303715884105727"}""", json);
        }

        [Fact]
        public void DeserializeInt128()
        {
            var json = """{"value":"170141183460469231731687303715884105727"}""";
            var result = JsonSerializer.Deserialize<Int128Wrap>(json);
            Assert.Equal(Int128.MaxValue, result.Value);
        }

        [Fact]
        public void RoundtripInt128()
        {
            var original = new Int128Wrap(Int128.Parse("123456789012345678901234567890"));
            var json = JsonSerializer.Serialize(original);
            var deserialized = JsonSerializer.Deserialize<Int128Wrap>(json);
            Assert.Equal(original.Value, deserialized.Value);
        }

        [Fact]
        public void SerializeUInt128()
        {
            var value = new UInt128Wrap(UInt128.MaxValue);
            var json = JsonSerializer.Serialize(value);
            Assert.Equal("""{"value":"340282366920938463463374607431768211455"}""", json);
        }

        [Fact]
        public void DeserializeUInt128()
        {
            var json = """{"value":"340282366920938463463374607431768211455"}""";
            var result = JsonSerializer.Deserialize<UInt128Wrap>(json);
            Assert.Equal(UInt128.MaxValue, result.Value);
        }

        [Fact]
        public void RoundtripUInt128()
        {
            var original = new UInt128Wrap(UInt128.Parse("123456789012345678901234567890"));
            var json = JsonSerializer.Serialize(original);
            var deserialized = JsonSerializer.Deserialize<UInt128Wrap>(json);
            Assert.Equal(original.Value, deserialized.Value);
        }

        [GenerateSerde]
        private partial record Int128Wrap(Int128 Value);

        [GenerateSerde]
        private partial record UInt128Wrap(UInt128 Value);
    }
}
