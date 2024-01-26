# Serde-dn

Serde-dn is a port of the popular [serde.rs](https://serde.rs) Rust ***ser***ialization/***de***serialization
library to .NET.

It is a multi-format serialization provider, meaning that it can support multiple output formats.

This package contains the core Serde library and support for JSON. Additional NuGet packages can provide support for additional formats.

## Getting started

You can now use the `[GenerateSerialize]` and `[GenerateDeserialize]` attributes to automatically implement serialization and
deserialization for your own types. Don't forget to mark them `partial`!

```csharp
using Serde;
using Serde.Json;

string output = JsonSerializer.Serialize(new SampleClass());

// prints: {"X":3,"Y":"sample"}
Console.WriteLine(output);

var deserialized = JsonSerializer.Deserialize<SampleClass>(output);

// prints SampleClass { X = 3, Y = sample }
Console.WriteLine(deserialized);

[GenerateSerialize, GenerateDeserialize]
partial record SampleClass
{
    // automatically includes public properties and fields
    public int X { get; init; } = 3;
    public string Y = "sample";
}
```
