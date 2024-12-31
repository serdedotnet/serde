
using System;
using Xunit;

namespace Serde;

public sealed class SourceBuilderTests
{

    [Fact]
    public void TestSourceBuilder()
    {
        var b = new SourceBuilder();
        int x = 10;
        b.Append($"Hello, {x}");
        Assert.Equal("Hello, 10", b.ToString());

        b = new();
        b.Append($$"""
{
    {{x}}
}
""");
        Assert.Equal("""
        {
            10
        }
        """, b.ToString());

        b = new($$"""
        {
            {{ "abc" + Environment.NewLine + "def"}}
        }
        """);
        Assert.Equal("""
            {
                abc
                def
            }
            """, b.ToString());
    }

    [Fact]
    public void BlankLinesNoIndent()
    {
        var s = """
            abc

            def
            """;
        var b = new SourceBuilder($$"""
        {
            {{s}}
        }
        """);
        Assert.Equal("""
        {
            abc

            def
        }
        """, b.ToString());
    }

}