﻿//HintName: A.B.C.D.IDeserialize.cs

#nullable enable

using System;
using Serde;
partial class A
{
    partial class B
    {
        partial class C
        {
            partial class D : Serde.IDeserializeProvider<A.B.C.D>
            {
                static IDeserialize<A.B.C.D> IDeserializeProvider<A.B.C.D>.DeserializeInstance
                    => _DeObj.Instance;

                sealed partial class _DeObj :Serde.IDeserialize<A.B.C.D>
                {
                    A.B.C.D Serde.IDeserialize<A.B.C.D>.Deserialize(IDeserializer deserializer)
                    {
                        int _l_field = default!;

                        byte _r_assignedValid = 0;

                        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<D>();
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
                    public static readonly _DeObj Instance = new();
                    private _DeObj() { }

                }
            }
        }
    }
}
