namespaceSerde.Test{partialclassJsonDeserializeTests{internal static class DenyUnknownSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<DenyUnknown>(nameof(DenyUnknown), new (string, System.Reflection.MemberInfo)[] {
        ("present", typeof(DenyUnknown).GetProperty("Present")!),
("missing", typeof(DenyUnknown).GetProperty("Missing")!)
    });
}}}