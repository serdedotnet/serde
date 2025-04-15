//HintName: SimpleRecord.ISerialize.cs

#nullable enable

using System;
using Serde;
partial record SimpleRecord : Serde.ISerializeProvider<SimpleRecord>
{
    static ISerialize<SimpleRecord> ISerializeProvider<SimpleRecord>.Instance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<SimpleRecord>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => SimpleRecord.s_serdeInfo;

        void global::Serde.ISerialize<SimpleRecord>.Serialize(SimpleRecord value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteI32(_l_info, 0, value.Id);
            _l_type.WriteString(_l_info, 1, value.Name);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
