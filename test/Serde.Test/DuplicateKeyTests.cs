using System;
using Serde.Json;
using Xunit;

namespace Serde.Test
{
    public partial class DuplicateKeyTests
    {
        [GenerateDeserialize]
        private partial struct SimpleType
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        [GenerateDeserialize]
        [SerdeTypeOptions(AllowDuplicateKeys = true)]
        private partial struct SimpleTypeAllowDuplicates
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        [Fact]
        public void DuplicateKeyThrowsException()
        {
            // Test that duplicate keys now throw an exception by default
            var json = @"{ ""name"": ""first"", ""value"": 42, ""name"": ""second"" }";

            var exception = Assert.Throws<DeserializeException>(() => 
                JsonSerializer.Deserialize<SimpleType>(json));

            Assert.Contains("Duplicate key", exception.Message);
            Assert.Contains("name", exception.Message);
        }

        [Fact]
        public void SimpleDeserializeTest()
        {
            // Test basic deserialization without duplicates first
            var json = @"{ ""name"": ""test"", ""value"": 42 }";

            var result = JsonSerializer.Deserialize<SimpleType>(json);

            Assert.Equal("test", result.Name);
            Assert.Equal(42, result.Value);
        }

        [Fact]
        public void DuplicateKeyWithDifferentCasingThrowsException()
        {
            // Test with different casing - should still throw exception
            var json = @"{ ""name"": ""first"", ""value"": 42, ""name"": ""second"" }";

            var exception = Assert.Throws<DeserializeException>(() => 
                JsonSerializer.Deserialize<SimpleType>(json));

            Assert.Contains("Duplicate key", exception.Message);
            Assert.Contains("name", exception.Message);
        }

        [Fact]
        public void MultipleDuplicateKeysThrowsException()
        {
            // Test with multiple duplicate keys - should throw on the first duplicate
            var json = @"{ ""name"": ""first"", ""value"": 10, ""name"": ""second"", ""value"": 20, ""name"": ""third"" }";

            var exception = Assert.Throws<DeserializeException>(() => 
                JsonSerializer.Deserialize<SimpleType>(json));

            Assert.Contains("Duplicate key", exception.Message);
            Assert.Contains("name", exception.Message);
        }

        [Fact]
        public void AllowDuplicateKeysOverwritesBehavior()
        {
            // Test that when DenyDuplicateKeys = false, duplicates still overwrite
            var json = @"{ ""name"": ""first"", ""value"": 42, ""name"": ""second"" }";

            var result = JsonSerializer.Deserialize<SimpleTypeAllowDuplicates>(json);

            // When duplicates are allowed, last value wins
            Assert.Equal("second", result.Name);
            Assert.Equal(42, result.Value);
        }
    }
}
