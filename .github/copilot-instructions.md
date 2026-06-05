# Copilot Instructions for serde.net

## Repository Overview

Serde.NET is a serialization/deserialization framework for .NET, inspired by Rust's serde.rs. It uses source generators to produce serialize/deserialize implementations at compile time.

### Project Structure

- `src/serde/` — Core library: interfaces (`ISerializer`, `IDeserializer`, `ITypeSerializer`, `ITypeDeserializer`), proxies, JSON implementation, and `ISerdeInfo` metadata.
- `src/generator/` — Roslyn source generator that auto-generates `ISerialize`/`IDeserialize` implementations from `[GenerateSerde]`.
- `pack/` — NuGet packaging project (`Serde.Pkg`).
- `test/Serde.Test/` — Unit tests including FsCheck property-based tests.
- `test/Serde.Generation.Test/` — Source generator snapshot tests using Verify.

### Build and Test

```bash
dotnet build -warnaserror serde-dn.sln   # Build all
dotnet test                               # Run all tests
./build.sh --pack                         # Build NuGet packages
```

## Adding New Primitive Types

When adding a new primitive type (e.g., `Half`/F16, `Int128`/I128):

1. Add the entry to the `PrimitiveKind` enum in `ISerdeInfo.cs` — **append at the end** to preserve numeric values.
2. Add `Read`/`Write` methods to the four core interfaces: `ISerializer`, `ITypeSerializer`, `IDeserializer`, `ITypeDeserializer`. **Use default interface implementations** to avoid breaking existing implementors.
3. Add a proxy class (e.g., `F16Proxy`) in `src/serde/Proxies.cs` implementing `ISerdePrimitive<TSelf, T>`.
4. Add proxy extensions in `src/serde/ProxyExtensions.cs` (guarded by `#if NET10_0_OR_GREATER`).
5. Update the JSON serializer/deserializer — all six files: `JsonSerializer.Serialize.cs` (ISerializer + ITypeSerializer + EnumSerializer), `JsonSerializer.Collection.cs` (EnumerableImpl, KeySerializer, DictImpl), `JsonDeserializer.cs`, `JsonDeserializer.Type.cs` (DeType), `JsonDeserializer.Collection.cs` (DeCollection).
6. Update the source generator in `src/generator/Proxies.cs` — add the type to `TryGetPrimitiveName` (for types with a `SpecialType`, use the switch; otherwise use name/namespace matching like `Int128`, `Half`, `DateTime`).
7. Add the type to the FsCheck generators in `test/Serde.Test/JsonFsCheck.cs`: add a `TestXxx` record and include it in `GenPrimitive`.

## Releasing

See [docs/releasing.md](../../docs/releasing.md) for the release process.
