# External types

Sometimes you need to serialize or deserialize a type you don't control or can't modify. In these cases, Serde uses a "proxy type" to stand-in for the external type. To create a proxy type, simply create a new class and add the attribute `[GenerateSerde(ForType = typeof(ExternalType)]`. Serde will automatically use the public properties and fields on the external type if the proxy type is empty. Here's a simple example that assumes there's an external `Response` record that you can't modify.

```csharp
{{#include ../samples/ExternalTypes.cs }}
```
