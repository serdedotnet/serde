# Using the source generator

Serialization in serde is driven by implementions of the `ISerde<T>` interface through a "serde object". Serde ships with a source generator that can automatically implement these interfaces for any type.

To use the source generator:

  1. Make the type `partial`.
  2. Add the `GenerateSerde`, `GenerateSerialize`, or `GenerateDeserialize` attribute

For example,

```csharp
class SampleClass
{
  ...
}
```

would become

```csharp
[GenerateSerde]
partial class SampleClass
{
  ...
}
```

By default, the source generator will include all the public properties *and* public fields. The field or property types must either,

  1. Directly implement the serde interfaces using `[GenerateSerde]`
  1. Be a serde-dn built-in type, like `int` or `string`.
  1. Specify a proxy using `[SerdeMemberOptions(Proxy = typeof(Proxy))]`

If you don't control any of the types you need to serialize or deserialize (e.g., they are defined in another assembly that you can't modify) you'll need to specify a *proxy*. See [External types](../foreign-types.md) for more info.

## Additional IDeserialize requirements

Deserialization needs a way create and initialize a given type. There are two recognized patterns:

  1. A parameterless constructor, where all the fields/properties are writable.
  2. A primary constructor, where all the fields/properties are either writable, or the same name as a one of the constructor parameters.

If this isn't possible, the constructor used can be configured through the `SerdeTypeOptions` attribute.