
using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace Serde.Test;

public sealed class IncrementalTests
{
    [Fact]
    public void GenerationOutputDeepEquals()
    {
        var gen1 = new GenerationOutput(
            Array.Empty<Diagnostic>(),
            new[] { ("Foo", new SourceBuilder("Bar")) });

        var gen2 = new GenerationOutput(
            Array.Empty<Diagnostic>(),
            new[] { ("Foo", new SourceBuilder("Bar")) });

        Assert.Equal(gen1, gen2);
    }
}