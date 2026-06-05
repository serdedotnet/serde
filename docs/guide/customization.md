# Customization

For complete control, you can disable source-generated implementation of the `ISerde<T>`/`ISerialize<T>`/`IDeserialize<T>` interfaces entirely and implement it yourself.

To specify the custom object, use `[GenerateSerde(With = typeof(CustomObject))]`. The custom type can be a private nested class, if desired.

Here is a simple example where Colors are converted to strings:

```csharp
{{#include ../samples/CustomSerialize.cs }}
```

In the above, the custom implementation also implements the required interface `ISerdeInfo`. The implementation of all these interfaces must match.