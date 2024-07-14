//HintName: BIND_OPTSSerdeInfo.cs
internal static class BIND_OPTSSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "BIND_OPTS",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("cbStruct", typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("cbStruct")!),
("dwTickCountDeadline", typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("dwTickCountDeadline")!),
("grfFlags", typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("grfFlags")!),
("grfMode", typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("grfMode")!)
    });
}