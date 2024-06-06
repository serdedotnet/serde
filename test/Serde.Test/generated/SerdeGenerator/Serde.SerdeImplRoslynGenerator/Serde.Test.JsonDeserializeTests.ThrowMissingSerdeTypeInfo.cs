namespaceSerde.Test{partialclassJsonDeserializeTests{internal static class ThrowMissingSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<ThrowMissing>(nameof(ThrowMissing), new (string, System.Reflection.MemberInfo)[] {
        ("present", typeof(ThrowMissing).GetProperty("Present")!),
("missing", typeof(ThrowMissing).GetProperty("Missing")!)
    });
}}}