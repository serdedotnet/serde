//HintName: ArgumentInfo.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class ArgumentInfo : Serde.ISerializeProvider<ArgumentInfo>
{
    static ISerialize<ArgumentInfo> ISerializeProvider<ArgumentInfo>.Instance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<ArgumentInfo>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => ArgumentInfo.s_serdeInfo;

        void global::Serde.ISerialize<ArgumentInfo>.Serialize(ArgumentInfo value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteString(_l_info, 0, value.Name);
            _l_type.WriteString(_l_info, 1, value.Value);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
