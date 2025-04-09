# Serde.NET

Serde.NET is a port of the popular [serde.rs](https://serde.rs) Rust ***ser***ialization/***de***serialization
library to .NET.

For an overview, see [Overview](https://serdedotnet.github.io/overview.html).

Start by adding the `serde` NuGet package:

```bash
dotnet add package serde
```

To generate serialization/deserialization info for a type, now do the following:
1. Mark the type partial
2. Add the `[GenerateSerde]` attribute


```csharp
using Serde;
using Serde.Json;

string output = JsonSerializer.Serialize(new SampleClass());

// prints: {"X":3,"Y":"sample"}
Console.WriteLine(output);

var deserialized = JsonSerializer.Deserialize<SampleClass>(output);

// prints SampleClass { X = 3, Y = sample }
Console.WriteLine(deserialized);

[GenerateSerde]
partial record SampleClass
{
    // automatically includes public properties and fields
    public int X { get; init; } = 3;
    public string Y = "sample";
}
```
