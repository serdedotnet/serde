namespaceSerde.Test{partialclassJsonDeserializeTests{internal static class ExtraMembersSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<ExtraMembers>(nameof(ExtraMembers), new (string, System.Reflection.MemberInfo)[] {
        ("b", typeof(ExtraMembers).GetField("b")!)
    });
}}}