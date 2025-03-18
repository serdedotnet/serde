//HintName: OPTSWrap.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class OPTSWrap : Serde.ISerializeProvider<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
{
    static ISerialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS> ISerializeProvider<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.SerializeInstance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
    {
        void global::Serde.ISerialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.Serialize(System.Runtime.InteropServices.ComTypes.BIND_OPTS value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<OPTSWrap>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteI32(_l_info, 0, value.cbStruct);
            _l_type.WriteI32(_l_info, 1, value.dwTickCountDeadline);
            _l_type.WriteI32(_l_info, 2, value.grfFlags);
            _l_type.WriteI32(_l_info, 3, value.grfMode);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
