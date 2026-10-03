//HintName: Shape._m_InstallProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial record Shape
{
    partial class _m_InstallProxy
    {
        sealed partial class _DeObj : Serde.IDeserialize<Shape.Install>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Shape._m_InstallProxy.s_serdeInfo;

            Shape.Install Serde.IDeserialize<Shape.Install>.Deserialize(IDeserializer deserializer)
            {
                bool _l_flag = true;
                int _l_fromprivateconst = 3;
                int _l_fromprivate = default!;
                int _l_frominternal = Shape.Install.Internal;

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
                            _l_flag = typeDeserialize.ReadBool(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                            _l_fromprivateconst = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                        case 2:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 2, _l_serdeInfo);
                            _l_fromprivate = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 2;
                            break;
                        case 3:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 3, _l_serdeInfo);
                            _l_frominternal = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 3;
                            break;
                        case Serde.ITypeDeserializer.IndexNotFound:
                            typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                typeDeserialize.End(_l_serdeInfo);
                if ((_r_assignedValid & 0b100) != 0b100)
                {
                    throw Serde.DeserializeException.UnassignedMember(_r_assignedValid, 0b100, _l_serdeInfo);
                }
                var newType = new Shape.Install() {
                    Flag = _l_flag,
                    FromPrivateConst = _l_fromprivateconst,
                    FromPrivate = _l_fromprivate,
                    FromInternal = _l_frominternal,
                };

                return newType;
            }
        }
    }
}
