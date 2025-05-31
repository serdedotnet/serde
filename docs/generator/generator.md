# Using the source generator

Serde-dn is type-safe, meaning that it requires every serializing type to implement `ISerialize` or `IDeserialize` and *always* uses an interface implementation for controlling serialization.

For types you control, you could implement those interfaces manually, but most cases can be solved by using the built-in source generator. Two changes are required to use the source generator:

  1. Make the type `partial`.
  2. Add the `GenerateSerialize` or `GenerateDeserialize` attribute.

For example,

```C#
class SampleClass
{
  ...
}
```

would become

```C#
[GenerateSerialize]
partial class SampleClass
{
  ...
}
```

By default, the source generator will include all the public properties *and* public fields. The field or property types must either,

  1. Be a serde-dn built-in type, like `int` or `string`.
  1. If the type is in the current assembly, implement `I(De)Serialize`.
  1. Be defined in an external assembly, where the source generator will automatically wrap the type (see [generated wrappers](./wrappers.md)).
  1. Define a [wrapper type](./wrappers.md) in the current assembly named `<TypeName>Wrap` in the `Serde` namespace.
  1. Specify a custom wrapper.

The types of the fields and properties must also implement ISerialize. Many of the most common types, like `int`, `string`, or even `List<string>`, have built-in support from serde-dn and already have ISerialize implementations. If any of those public fields or properties have a type that you control, they will also need to implement `ISerialize` or `IDeserialize`. This is also true for nested references, e.g. `List<MyOtherType>`.

If you don't control any of the types you need to serialize or deserialize (i.e., they are defined in another assembly that you can't modify) you'll need to use a *wrapper* to implement the interface. See [wrappers](./wrappers.md) for more info.

## Additional IDeserialize requirements

Serde-dn is type safe and always uses reflection-free C# code. Thus, to implement IDeserialize the type must have an accessible constructor and accessible members.

By default, serde-dn requires

  1. A parameterless constructor
  2. All fields and properties are writable during creation.

If this isn't possible, the constructor used and the properties can be configured through the `SerdeTypeOptions` attribute.