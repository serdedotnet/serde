# Getting Started

Serde-dn is a port of the popular [serde.rs](https://serde.rs) Rust serialization/deserialization
library to .NET.

Serde-dn only supports serialization at the moment. Deserialization support is under construction.

Start by adding the `serde-dn` NuGet package:

```bash
dotnet add package serde-dn --prerelease
```

Next, your type needs to implement the `ISerialize` interface. Serde-dn will do this for you if your
type is marked `partial` and attributed with `[GenerateSerialize]`.  You can then pass the type to
any implementation of `ISerializer`. Serde-dn ships with a JSON serializer that can serialize to
strings.

```csharp
[GenerateSerialize]
partial class SampleClass
{     
    // automatically includes public properties and fields  
    public int X { get; } = 3;
    public string Y = "sample";
}

Console.WriteLine(Serde.Json.JsonSerializer.Serialize(new SampleClass()));
// prints: {"X":3,"Y":"sample"}
```