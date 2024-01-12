//HintName: OPTSWrap.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial record struct OPTSWrap : Serde.ISerialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
{
    void ISerialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.Serialize(System.Runtime.InteropServices.ComTypes.BIND_OPTS value, ISerializer serializer)
    {
        var type = serializer.SerializeType("BIND_OPTS", 4);
        type.SerializeField<int, Int32Wrap>("cbStruct", value.cbStruct);
        type.SerializeField<int, Int32Wrap>("dwTickCountDeadline", value.dwTickCountDeadline);
        type.SerializeField<int, Int32Wrap>("grfFlags", value.grfFlags);
        type.SerializeField<int, Int32Wrap>("grfMode", value.grfMode);
        type.End();
    }
}