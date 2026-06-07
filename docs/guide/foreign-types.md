# External types

Sometimes you need to serialize or deserialize a type you don't control or can't modify. In these cases, Serde uses a "proxy type" to stand-in for the external type. To create a proxy type, simply create a new class and add the attribute `[GenerateSerde(ForType = typeof(ExternalType)]`. Serde will automatically use the public properties and fields on the external type if the proxy type is empty. Here's a simple example that assumes there's an external `Response` record that you can't modify.

```csharp
{{#include ../../samples/ExternalTypes.cs }}
```

## Types that need conversion

An empty proxy works when the external type has a parameterless constructor (or a
matching primary constructor) so Serde can construct it during deserialization.
Some external types don't fit that mold — for example, a BCL type like
`System.Version` ships without any Serde support, can't be annotated with
`[GenerateSerde]`, and has no parameterless constructor. For those, write a
*non-empty* proxy that mirrors the data you want on the wire, and provide explicit
conversion operators in both directions:

- `ExternalType -> Proxy`, used when **serializing**.
- `Proxy -> ExternalType`, used when **deserializing**.

Serde generates the serialization against the proxy's own fields and uses the
operators to convert to and from the external type at the call site. The operators
are the contract that the proxy faithfully represents the external type, and the
compiler enforces that they exist: if you generate serialization you must supply
`ExternalType -> Proxy`, and if you generate deserialization you must supply
`Proxy -> ExternalType`.

```csharp
{{#include ../../samples/VersionProxy.cs }}
```

Serializing a `Version` then produces JSON from the proxy's fields:

```json
{"major":1,"minor":2,"build":3,"revision":4}
```

Note that the proxy only needs the conversion operators for the directions you
actually generate. A serialize-only proxy (`[GenerateSerialize(ForType = ...)]`)
needs only `ExternalType -> Proxy`, and a deserialize-only proxy
(`[GenerateDeserialize(ForType = ...)]`) needs only `Proxy -> ExternalType`.
