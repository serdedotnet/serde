//HintName: BIND_OPTSSerdeTypeInfo.cs
internal static class BIND_OPTSSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("cbStruct", typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("cbStruct")!),
("dwTickCountDeadline", typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("dwTickCountDeadline")!),
("grfFlags", typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("grfFlags")!),
("grfMode", typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("grfMode")!)
    });
}