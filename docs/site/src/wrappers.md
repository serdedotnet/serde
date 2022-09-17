# Wrappers

Wrappers are an essential part of the serde-dn design. Many types aren't under user control and won't be able to implement the `ISerialize` or `IDeserialize` interfaces directly. Instead, simple wrapper structs are used to proxy the target type.

Often, wrappers will be created for you as necessary. If you are serializing a type you define, the source generator will automatically create wrappers for nested external types. However, wrappers will never be automatically created for types in your source code. If you control the type, it must implement the required interfaces directly.

## Creating a wrapper type

By convention, wrappers are named `<OriginalTypeName>Wrap` and are always placed in the `Serde` namespace. Since wrappers are normal C#, you could choose to implement the `ISerialize` or `IDeserialize` logic yourself, just as you would for a custom implementation on your own type. However, the serde-dn source generator also has support for generating implementations for wrappers.

To use implement via source generator do the following:

  1. Create a wrapper type (often a struct) named `<OriginalTypeName>Wrap` in the `Serde` namespace.
  1. Make it `partial`.
  1. Add a field or property to store the wrapped instance.
  1. Add the `[GenerateWrapper]` attribute and pass it the name of your field or property as a string (maybe using `nameof`)

The "records" feature is particularly useful for this:

```C#
using Serde;

// Type we can't modify
class ExternalClass { ... }

namespace Serde
{
    [GenerateWrapper(nameof(Value))]
    partial readonly record struct ExternalClassWrap(ExternalClass Value);
}
```

By default, this will implement both `ISerialize` and `IDeserialize` in the wrapper.