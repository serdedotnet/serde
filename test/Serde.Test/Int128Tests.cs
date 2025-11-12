
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
            var value = Int128.MaxValue;
            var json = JsonSerializer.Serialize<Int128, I128Proxy>(value);
            Assert.Equal("\"170141183460469231731687303715884105727\"", json);
        }

        [Fact]
        public void DeserializeInt128()
        {
            var json = "\"170141183460469231731687303715884105727\"";
            var result = JsonSerializer.Deserialize<Int128, I128Proxy>(json);
            Assert.Equal(Int128.MaxValue, result);
        }

        [Fact]
        public void RoundtripInt128()
        {
            var original = Int128.Parse("123456789012345678901234567890");
            var json = JsonSerializer.Serialize<Int128, I128Proxy>(original);
            var deserialized = JsonSerializer.Deserialize<Int128, I128Proxy>(json);
            Assert.Equal(original, deserialized);
        }

        [Fact]
        public void SerializeUInt128()
        {
            var value = UInt128.MaxValue;
            var json = JsonSerializer.Serialize<UInt128, U128Proxy>(value);
            Assert.Equal("\"340282366920938463463374607431768211455\"", json);
        }

        [Fact]
        public void DeserializeUInt128()
        {
            var json = "\"340282366920938463463374607431768211455\"";
            var result = JsonSerializer.Deserialize<UInt128, U128Proxy>(json);
            Assert.Equal(UInt128.MaxValue, result);
        }

        [Fact]
        public void RoundtripUInt128()
        {
            var original = UInt128.Parse("123456789012345678901234567890");
            var json = JsonSerializer.Serialize<UInt128, U128Proxy>(original);
            var deserialized = JsonSerializer.Deserialize<UInt128, U128Proxy>(json);
            Assert.Equal(original, deserialized);
        }
    }
}
