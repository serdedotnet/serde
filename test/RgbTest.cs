using System;
using Xunit;

namespace Serde.Test
{
    public class JsonTests
    {
        [Fact]
        public void Rgb()
        {
            Rgb value = new Rgb { Red = 10, Green = 30, Blue = 20 };
            Assert.Equal("{Red:10,Green:30,Blue:20}", JsonSerializer.ToString(value));
        }
    }
}
