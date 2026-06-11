//HintName: C.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class C
{
    sealed partial class _DeObj : Serde.IDeserialize<C>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => C.s_serdeInfo;

        C Serde.IDeserialize<C>.Deserialize(IDeserializer deserializer)
        {
            string _l_str = "hello";
            int _l_num = 42;
            bool _l_flag = true;
            string? _l_nullable = null;
            double _l_dbl = 3.14;
            char _l_ch = 'x';
            string _l_frommethod = string.Empty;
            int _l_maxint = int.MaxValue;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            while (true)
            {
                var (_l_index_, _) = typeDeserialize.TryReadIndexWithName(_l_serdeInfo);
                if (_l_index_ == Serde.ITypeDeserializer.EndOfType)
                {
                    break;
                }

                switch (_l_index_)
                {
                    case 0:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 0, _l_serdeInfo);
                        _l_str = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                        _l_num = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 2:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 2, _l_serdeInfo);
                        _l_flag = typeDeserialize.ReadBool(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case 3:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 3, _l_serdeInfo);
                        _l_nullable = typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 3;
                        break;
                    case 4:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 4, _l_serdeInfo);
                        _l_dbl = typeDeserialize.ReadF64(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 4;
                        break;
                    case 5:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 5, _l_serdeInfo);
                        _l_ch = typeDeserialize.ReadChar(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 5;
                        break;
                    case 6:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 6, _l_serdeInfo);
                        _l_frommethod = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 6;
                        break;
                    case 7:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 7, _l_serdeInfo);
                        _l_maxint = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 7;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b11110111) != 0b11110111)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new C() {
                Str = _l_str,
                Num = _l_num,
                Flag = _l_flag,
                Nullable = _l_nullable,
                Dbl = _l_dbl,
                Ch = _l_ch,
                FromMethod = _l_frommethod,
                MaxInt = _l_maxint,
            };

            return newType;
        }
    }
}
