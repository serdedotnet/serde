//HintName: Some.Nested.Namespace.Base._m_BProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_BProxy
    {
        sealed partial class _DeObj : Serde.IDeserialize<Some.Nested.Namespace.Base.B>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Some.Nested.Namespace.Base._m_BProxy.s_serdeInfo;

            Some.Nested.Namespace.Base.B Serde.IDeserialize<Some.Nested.Namespace.Base.B>.Deserialize(IDeserializer deserializer)
            {
                string _l_y = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_y = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
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
                var newType = new Some.Nested.Namespace.Base.B(_l_y) {
                };

                return newType;
            }
        }
    }
}
