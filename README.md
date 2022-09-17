# Serde-dn

Serde-dn is a port of the popular [serde.rs](https://serde.rs) Rust ***ser***ialization/***de***serialization
library to .NET.

For an overview, see [Overview](https://agocke.github.io/serde-dn/overview.html).

Start by adding the `serde-dn` NuGet package:

```bash
dotnet add package serde --prerelease
```

Serde-dn uses C# 11 features, so until .NET 7 is released it requires the following flags to be added to the `csproj` file:

```
<LangVersion>preview</LangVersion>
<EnablePreviewFeatures>true</EnablePreviewFeatures>
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
