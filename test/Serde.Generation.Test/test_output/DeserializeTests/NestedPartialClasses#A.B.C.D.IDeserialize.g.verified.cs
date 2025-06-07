//HintName: A.B.C.D.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class A
{
    partial class B
    {
        partial class C
        {
            partial class D
            {
                sealed partial class _DeObj : Serde.IDeserialize<A.B.C.D>
                {
                    global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => A.B.C.D.s_serdeInfo;

                    A.B.C.D Serde.IDeserialize<A.B.C.D>.Deserialize(IDeserializer deserializer)
                    {
                        int _l_field = default!;

                        byte _r_assignedValid = 0;

                        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                        var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                        int _l_index_;
                        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
                        {
                            switch (_l_index_)
                            {
                                case 0:
                                    _l_field = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
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
                        var newType = new A.B.C.D() {
                            Field = _l_field,
                        };

                        return newType;
                    }
                }
            }
        }
    }
}
