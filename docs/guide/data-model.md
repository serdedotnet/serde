# Data model

Serde works by taking .NET code, transforming it into the Serde data model, and then translating it into the various output formats. The Serde data model is a subset of the .NET type system, focused on only the pieces that can be translated to serialization output formats.

In code, the Serde data model is split into two sides: .NET types and output formats. .NET types implement the `ISerialize<T>`, `IDeserialialize<T>`, and `ISerdeInfo` interfaces. Output formats implement the `ISerializer` and `IDeserializer` interfaces.

The Serde data model is as follows:

- **Primitive types**
    - `bool`
    - `I8`, `I16`, `I32`, and `I64` (`sbyte`, `short`, `int`, and `long`)
    - `U8`, `U16`, `U32`, and `U64` (`byte`, `ushort`, `uint`, and `ulong`)
    - `F32` and `F64` (`float` and `double`)
    - `decimal`
    - `string`
    - `DateTime`
- **Contiguous array of bytes**
  - Serialization: `ReadOnlyMemory<byte>`
  - Deserialization: `IBufferWriter<byte>`
- **Custom types**
  - Structs, classes, and records.
- **Lists**
  - A sequence of elements, like an array or a list. The length does not need to be known ahead-of-time.
- **Dictionaries**
  - A variable-size key-value pair. The length does not need to be known ahead-of-time.
- **Nullable types**
  - Both nullable reference types and nullable value types are supported. All reference types not marked as nullable (`?` suffix) are assumed to be non-null.
- **Enums**
- **Unions**
  - Sometimes referred to as "polymorphic types." C# does not yet have built-in support for discriminated unions, but the source generator will match a particular pattern anyway. To write a union type, use an abstract record type, with nested records that inherit from the parent record. The parent record must only have private constructors. For example,

```csharp
{{#include ../samples/Unions.cs:union-def}}
```