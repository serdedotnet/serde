﻿//HintName: Some.Nested.Namespace.Base._m_BProxy.ISerde.g.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_BProxy
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Some.Nested.Namespace.Base.B>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Some.Nested.Namespace.Base._m_BProxy.s_serdeInfo;

            void global::Serde.ISerialize<Some.Nested.Namespace.Base.B>.Serialize(Some.Nested.Namespace.Base.B value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteString(_l_info, 0, value.Y);
                _l_type.End(_l_info);
            }
            Some.Nested.Namespace.Base.B Serde.IDeserialize<Some.Nested.Namespace.Base.B>.Deserialize(IDeserializer deserializer)
            {
                string _l_y = default!;

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
