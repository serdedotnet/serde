# Customization

Serde gives you several ways to control how a type is serialized, ranging from small tweaks to a completely hand-written implementation. Use this table to pick the right one:

| Situation | Use | More info |
|---|---|---|
| You own the type and the default member-by-member shape is fine | `[GenerateSerde]` | [Using the source generator](./generator/generator.md) |
| You own the type but want to rename members, skip nulls, etc. | `[SerdeTypeOptions]` / `[SerdeMemberOptions]` | [Configuration options](./generator/options.md#type-options) |
| You own the type and want it on the wire *as* another serializable type (e.g. a primitive), via conversion operators | `[GenerateSerde(As = typeof(...))]` | [Configuration options](./generator/options.md#direct-generate-options) |
| You own the type and need full manual control of the wire format | `[GenerateSerde(With = typeof(...))]` | [Below](#full-manual-control-with-with) |
| You don't own the type, but it has a usable constructor and the default shape is fine | empty `[GenerateSerde(ForType = typeof(...))]` proxy | [External types](./foreign-types.md) |
| You don't own the type and it needs a custom shape or has no usable constructor | non-empty `[GenerateSerde(ForType = typeof(...))]` proxy with conversion operators | [External types](./foreign-types.md#types-that-need-conversion) |

> **Note:** `As` and the non-empty `ForType` proxy are the same underlying mechanism — user-defined conversion operators to a "wire" type. The only difference is whether you own the declaring type (use `As`) or not (use a `ForType` proxy).

## Full manual control with `With`

For complete control, you can disable source-generated implementation of the `ISerde<T>`/`ISerialize<T>`/`IDeserialize<T>` interfaces entirely and implement it yourself.

To specify the custom object, use `[GenerateSerde(With = typeof(CustomObject))]`. The custom type can be a private nested class, if desired.

Here is a simple example where Colors are converted to strings:

```csharp
{{#include ../../samples/CustomSerialize.cs }}
```

In the above, the custom implementation also implements the required interface `ISerdeInfo`. The implementation of all these interfaces must match.