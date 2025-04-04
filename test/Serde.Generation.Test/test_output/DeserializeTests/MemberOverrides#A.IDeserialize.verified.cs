﻿//HintName: A.IDeserialize.cs

#nullable enable

using System;
using Serde;
partial record A : Serde.IDeserializeProvider<A>
{
    static IDeserialize<A> IDeserializeProvider<A>.Instance
        => _DeObj.Instance;

    sealed partial class _DeObj :Serde.IDeserialize<A>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => A.s_serdeInfo;

        A Serde.IDeserialize<A>.Deserialize(IDeserializer deserializer)
        {
            int _l_x = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_x = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b1) != 0b1)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new A(_l_x) {
            };

            return newType;
        }
        public static readonly _DeObj Instance = new();
        private _DeObj() { }

    }
}
