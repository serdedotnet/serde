//HintName: SetToNullSerdeInfo.cs
internal static class SetToNullSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "SetToNull",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("present", typeof(SetToNull).GetProperty("Present")!),
("missing", typeof(SetToNull).GetProperty("Missing")!),
("throwMissing", typeof(SetToNull).GetProperty("ThrowMissing")!)
    });
}