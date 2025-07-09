﻿//HintName: B.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class B
{
    sealed partial class _DeObj : Serde.IDeserialize<B>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => B.s_serdeInfo;

        async global::System.Threading.Tasks.Task<B> Serde.IDeserialize<B>.Deserialize(IDeserializer deserializer)
        {
            int _l_y = default!;
            int _l_x = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            while (true)
            {
                var (_l_index_, _) = await typeDeserialize.TryReadIndexWithName(_l_serdeInfo);
                if (_l_index_ == Serde.ITypeDeserializer.EndOfType)
                {
                    break;
                }

                switch (_l_index_)
                {
                    case 0:
                        _l_y = await typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        _l_x = await typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b11) != 0b11)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new B() {
                Y = _l_y,
                X = _l_x,
            };

            return newType;
        }
    }
}
