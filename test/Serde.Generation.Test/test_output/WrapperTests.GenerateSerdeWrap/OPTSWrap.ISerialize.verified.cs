//HintName: OPTSWrap.ISerialize.cs

#nullable enable

using System;
using Serde;
partial record struct OPTSWrap : Serde.ISerializeProvider<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
{
    static ISerialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS> ISerializeProvider<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.SerializeInstance
        => OPTSWrapSerializeProxy.Instance;

    sealed partial class OPTSWrapSerializeProxy :Serde.ISerialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
    {
        void global::Serde.ISerialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.Serialize(System.Runtime.InteropServices.ComTypes.BIND_OPTS value, global::Serde.ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<OPTSWrap>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<int,global::Serde.Int32Proxy>(_l_serdeInfo,0,value.cbStruct);
            type.SerializeField<int,global::Serde.Int32Proxy>(_l_serdeInfo,1,value.dwTickCountDeadline);
            type.SerializeField<int,global::Serde.Int32Proxy>(_l_serdeInfo,2,value.grfFlags);
            type.SerializeField<int,global::Serde.Int32Proxy>(_l_serdeInfo,3,value.grfMode);
            type.End();
        }
        public static readonly OPTSWrapSerializeProxy Instance = new();
        private OPTSWrapSerializeProxy() { }

    }
}
