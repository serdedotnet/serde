
# Configuration options

Serde-dn provides a variety of options that can be specified via attributes to configure the generated serialization and deserialization implementations.

## Direct Generate options

The `GenerateSerde`, `GenerateSerialize`, and `GenerateDeserialize` attributes have a few options to select a custom implementation.

- `[GenerateSerde(ForType = typeof(...))]`

  Used to create proxy types. See [External types](../foreign-types.md) for more info.

- `[GenerateSerde(With = typeof(...))]`

  Used to override the generation of the serde object with the specified custom serde object. The target type needs to implement `ISerde<T>`.

- `[GenerateSerde(As = typeof(...))]`

  Serializes and deserializes the declaring type *as* the given type by going through user-defined
  conversions. When serializing, the declaring type is converted to the target type and serialized
  as that type; when deserializing, the target type is deserialized and converted back to the
  declaring type. User-defined conversions (implicit or explicit) must exist in the directions
  required by the usage: declaring type → target type for serialization, and target type →
  declaring type for deserialization. This is useful when a type should be represented on
  the wire as a primitive or another serializable type. `As` cannot be combined with `ForType` or
  `With`, and cannot be applied to enums.

  ```csharp
  [GenerateSerde(As = typeof(string))]
  public readonly partial struct Rgb
  {
      public readonly byte R, G, B;
      public Rgb(byte r, byte g, byte b) => (R, G, B) = (r, g, b);

      public static explicit operator string(Rgb c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";
      public static explicit operator Rgb(string s) => /* parse "#RRGGBB" */;
  }
  ```

## Type options

To apply options to an entire type and all its members, use `[SerdeTypeOptions]`. To apply options to one member in particular, use `[SerdeMemberOptions]`.

Note that these options only apply to the target type, not the type of nested members (including ones which have wrappers auto-generated). To provide options for member types the attribute will also need to be applied to them (or their wrapper).

- `[SerdeTypeOptions(MemberFormat = MemberFormat.CamelCase)]`

  `MemberFormat.CamelCase` by default. Renames all the fields or properties of the generated implementation according to the given format. The possible formats are "camelCase", "PascalCase", "kebab-case", "snake_case", and "none". "none" means that the members should not be renamed.

- `[SerdeTypeOptions(SerializeNull = false)]`

  `false` by default. When false, serialization for members will be skipped if the value is null. When true, null will be serialized like all other values.

- `[SerdeTypeOptions(DenyUnknownMembers = false)]`

  `false` by default. When false, the generated implementation of `IDeserialize` will skip over any members in the source that aren't defined on the type. When true, an exception will be thrown if there are any unrecognized members in the source.

- `[SerdeTypeOptions(AllowDuplicateKeys = false)]`

  `false` by default. When false, the generated implementation of `IDeserialize` will throw an exception if duplicate keys are encountered during deserialization. When true, duplicate keys will overwrite previous values (last value wins behavior).

## Member options

- `[SerdeMemberOptions(ThrowIfMissing = false)]`

  `false` by default. When true, throws an exception if the target field is not present when deserializing.  This is the default behavior for fields of non-nullable types, while the default behavior for nullable types is to set the field to null.

- `[SerdeMemberOptions(SerializeNull = false)]`

  `false` by default. When false, serialization for this member will be skipped if the value is null. When true, null will be serialized like all other values.

- `[SerdeMemberOptions(Rename = "name")]`

  `null` by default. When not null, renames the current member to the given argument.