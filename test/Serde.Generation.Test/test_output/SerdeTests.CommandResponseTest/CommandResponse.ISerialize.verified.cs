//HintName: CommandResponse.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class CommandResponse<TResult, TProxy> : Serde.ISerializeProvider<CommandResponse<TResult, TProxy>>
{
    static ISerialize<CommandResponse<TResult, TProxy>> ISerializeProvider<CommandResponse<TResult, TProxy>>.Instance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<CommandResponse<TResult, TProxy>>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => CommandResponse<TResult, TProxy>.s_serdeInfo;

        void global::Serde.ISerialize<CommandResponse<TResult, TProxy>>.Serialize(CommandResponse<TResult, TProxy> value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteI32(_l_info, 0, value.Status);
            _l_type.WriteString(_l_info, 1, value.Message);
            _l_type.WriteValueIfNotNull<System.Collections.Generic.List<ArgumentInfo>?, Serde.NullableRefProxy.Ser<System.Collections.Generic.List<ArgumentInfo>, Serde.ListProxy.Ser<ArgumentInfo, ArgumentInfo>>>(_l_info, 2, value.Arguments);
            _l_type.WriteValue<TResult, TProxy>(_l_info, 3, value.Results);
            _l_type.WriteI64(_l_info, 4, value.Duration);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
