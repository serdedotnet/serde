//HintName: ComplexRecord.ISerialize.cs

#nullable enable

using System;
using Serde;
partial record ComplexRecord : Serde.ISerializeProvider<ComplexRecord>
{
    static ISerialize<ComplexRecord> ISerializeProvider<ComplexRecord>.Instance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<ComplexRecord>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => ComplexRecord.s_serdeInfo;

        void global::Serde.ISerialize<ComplexRecord>.Serialize(ComplexRecord value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteI32(_l_info, 0, value.Id);
            _l_type.WriteStringIfNotNull(_l_info, 1, value.Description);
            _l_type.WriteValueIfNotNull<SimpleRecord?, Serde.NullableRefProxy.Ser<SimpleRecord, SimpleRecord>>(_l_info, 2, value.NestedRecord);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
