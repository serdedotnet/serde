//HintName: CommandResponse.IDeserialize.cs

#nullable enable

using System;
using Serde;
partial class CommandResponse<TResult, TProxy> : Serde.IDeserializeProvider<CommandResponse<TResult, TProxy>>
{
    static IDeserialize<CommandResponse<TResult, TProxy>> IDeserializeProvider<CommandResponse<TResult, TProxy>>.Instance
        => _DeObj.Instance;

    sealed partial class _DeObj :Serde.IDeserialize<CommandResponse<TResult, TProxy>>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => CommandResponse<TResult, TProxy>.s_serdeInfo;

        CommandResponse<TResult, TProxy> Serde.IDeserialize<CommandResponse<TResult, TProxy>>.Deserialize(IDeserializer deserializer)
        {
            int _l_status = default!;
            string _l_message = default!;
            System.Collections.Generic.List<ArgumentInfo>? _l_arguments = default!;
            TResult _l_results = default!;
            long _l_duration = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_status = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        _l_message = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 2:
                        _l_arguments = typeDeserialize.ReadValue<System.Collections.Generic.List<ArgumentInfo>?, Serde.NullableRefProxy.De<System.Collections.Generic.List<ArgumentInfo>, Serde.ListProxy.De<ArgumentInfo, ArgumentInfo>>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case 3:
                        _l_results = typeDeserialize.ReadValue<TResult, TProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 3;
                        break;
                    case 4:
                        _l_duration = typeDeserialize.ReadI64(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 4;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b11011) != 0b11011)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new CommandResponse<TResult, TProxy>() {
                Status = _l_status,
                Message = _l_message,
                Arguments = _l_arguments,
                Results = _l_results,
                Duration = _l_duration,
            };

            return newType;
        }
        public static readonly _DeObj Instance = new();
        private _DeObj() { }

    }
}
