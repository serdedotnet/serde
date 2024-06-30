//HintName: OPTSWrap.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial record struct OPTSWrap : Serde.ISerialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
{
    void ISerialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.Serialize(System.Runtime.InteropServices.ComTypes.BIND_OPTS value, ISerializer serializer)
    {
        var _l_typeInfo = BIND_OPTSSerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeField<int, Int32Wrap>(_l_typeInfo, 0, value.cbStruct);
        type.SerializeField<int, Int32Wrap>(_l_typeInfo, 1, value.dwTickCountDeadline);
        type.SerializeField<int, Int32Wrap>(_l_typeInfo, 2, value.grfFlags);
        type.SerializeField<int, Int32Wrap>(_l_typeInfo, 3, value.grfMode);
        type.End();
    }
}