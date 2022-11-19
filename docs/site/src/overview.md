<div style="text-align: right;">
  <a style="color: var(--icons);" href="https://github.com/agocke/serde-dn">View on GitHub</a>
</div>

# Overview

Serde-dn is a .NET port of the popular [serde.rs](https://serde.rs) Rust **ser**ialization/**de**serialization library.

## Design

Unlike many other serialization libraries, serde-dn is *multi-format*, *high performance*, *source-generated*, and fully compatible with [app trimming](https://docs.microsoft.com/en-us/dotnet/core/deploying/trimming/trim-self-contained) and [Native AOT](https://docs.microsoft.com/en-us/dotnet/core/deploying/native-aot).

Most other .NET serialization libraries rely on run-time reflection to serialize types. Serde-dn instead uses two interfaces, `ISerialize`, and `IDeserialize` to allow each type to control how it is serialized. Serde-dn uses source generation to implement these interfaces, so you almost never need to implement them manually. Source generation and interfaces avoids all run-time reflection and its overhead, and provides type safety by ensuring all requested types support serialization.

## Formats

Serde-dn is a multi-format serialization library, with built-in support for JSON.

Serde-dn is multi-format because it separates how a type serializes itself from the knowledge of the data format. Rather than have individual interfaces for each format, serde-dn uses `ISerialize` and `IDeserialize` for all formats. Adding support for new formats comes from implementing two different interfaces: `ISerializer` and `IDeserializer`. Since each interface pair is separate, new data format support can be added via NuGet packages.


## Getting started

This full sample is also available in the [Github repo](https://github.com/agocke/serde-dn/tree/main/samples/intro).

Start by adding the `serde` NuGet package:

```bash
dotnet add package serde
```

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