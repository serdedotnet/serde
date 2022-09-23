
# Configuration options

Serde-dn provides a variety of options that can be specified via attributes to configure the generated serialization and deserialization implementations.

To apply options to an entire type and all its members, use `[SerdeTypeOptions]`. To apply options to one member in particular, use `[SerdeMemberOptions]`.

## Type options

Note that these options only apply to the target type, not the type of nested members (including ones which have wrappers auto-generated). To provide options for member types the attribute will also need to be applied to them (or their wrapper).

- `[SerdeTypeOptions(MemberFormat = ...)]`

  `MemberFormat.CamelCase` by default. Renames all the fields or properties of the generated implementation according to the given format. The possible formats are "camelCase", "PascalCase", "kebab-case", and "none". "none" means that the members should not be renamed.

- `[SerdeTypeOptions(SerializeNull = false)]`

  `false` by default. When false, serialization for members will be skipped if the value is null. When true, null will be serialized like all other values.

- `[SerdeTypeOptions(DenyUnknownMembers = false)]`

  `false` by default. When false, the generated implementation of `IDeserialize` will skip over any members in the source that aren't defined on the type. When true, an exception will be thrown if there are any unrecognized members in the source.

## Member options

- `[SerdeMemberOptions(ThrowIfMissing = false)]`

  `false` by default. When true, throws an exception if the target field is not present when deserializing.  This is the default behavior for fields of non-nullable types, while the default behavior for nullable types is to set the field to null.

- `[SerdeMemberOptions(SerializeNull = false)]`

  `false` by default. When false, serialization for this member will be skipped if the value is null. When true, null will be serialized like all other values.

- `[SerdeMemberOptions(Rename = "name")]`

  `null` by default. When not null, renames the current member to the given argument.