# Using the source generator

Serde-dn is type-safe, meaning that it requires every serializing type to implement `ISerialize` or `IDeserialize` and *always* uses an interface implementation for controlling serialization.

For types you control, you could implement those interfaces manually, but most cases can be solved by using the built-in source generator. To have the source generator implement the appropriate interface, start by making the type `partial`. For example, if you had a struct or class like

```C#
class SampleClass
{
  ...
}
```

you would add the `partial` keyword before the class definition, i.e.

```C#
partial class SampleClass
{
  ...
}
```

Next, add the `[GenerateSerialize]` and/or the `[GenerateDeserialize]` attribute.

```C#
[GenerateSerialize]
partial class SampleClass
{
  ...
}
```

By default, this will include all the public properties *and* public fields. The types of the fields and properties must also implement ISerialize. Many of the most common types, like `int`, `string`, or even `List<string>`, have built-in support from serde-dn and already have ISerialize implementations. If any of those public fields or properties have a type that you control, they will also need to implement `ISerialize` or `IDeserialize`. This is also true for nested references, e.g. `List<MyOtherType>`.

If you don't control any of the types you need to serialize or deserialize (i.e., they are defined in another assembly that you can't modify) you'll need to use a *wrapper* to implement the interface. See [wrappers](./wrappers.md) for more info.

To serialize or deserialize this type, you'll need to pass it into the appropriate serializer or deserializer. For example, serde-dn includes
a JSON serializer `Serde.Json.JsonSerializer` which defines `Serialize` and `Deserialize` static methods. Serialize takes an instance of `ISerialize` as a parameter, while `Deserialize` takes a type implementing `IDeserialize` as a *type* parameter. In both cases, all you need is to implement the appropriate interface on your type.

## Additional IDeserialize requirements

Serde-dn is type safe and always uses reflection-free C# code. Thus, to implement IDeserialize the type must have an accessible constructor and accessible members.

By default, serde-dn requires
  1. A parameterless constructor
  2. All fields and properties are writable during creation.