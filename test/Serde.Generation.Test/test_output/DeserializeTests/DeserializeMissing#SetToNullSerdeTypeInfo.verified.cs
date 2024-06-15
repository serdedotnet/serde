//HintName: SetToNullSerdeTypeInfo.cs
internal static class SetToNullSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "SetToNull",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("present", typeof(SetToNull).GetProperty("Present")!),
("missing", typeof(SetToNull).GetProperty("Missing")!),
("throwMissing", typeof(SetToNull).GetProperty("ThrowMissing")!)
    });
}