//HintName: Some.Nested.Namespace.Base._m_AProxy.ISerde.g.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial record Base
{
    partial class _m_AProxy
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Some.Nested.Namespace.Base.A>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Some.Nested.Namespace.Base._m_AProxy.s_serdeInfo;

            void global::Serde.ISerialize<Some.Nested.Namespace.Base.A>.Serialize(Some.Nested.Namespace.Base.A value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteI32(_l_info, 0, value.X);
                _l_type.End(_l_info);
            }
            async global::System.Threading.Tasks.Task<Some.Nested.Namespace.Base.A> Serde.IDeserialize<Some.Nested.Namespace.Base.A>.Deserialize(IDeserializer deserializer)
            {
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
                            _l_x = await typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case Serde.ITypeDeserializer.IndexNotFound:
                            await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                if ((_r_assignedValid & 0b1) != 0b1)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Some.Nested.Namespace.Base.A(_l_x) {
                };

                return newType;
            }
        }
    }
}
