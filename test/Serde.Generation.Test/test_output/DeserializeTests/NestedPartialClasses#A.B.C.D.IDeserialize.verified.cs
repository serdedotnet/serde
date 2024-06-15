//HintName: A.B.C.D.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial class A
{
    partial class B
    {
        partial class C
        {
            partial class D : Serde.IDeserialize<A.B.C.D>
            {
                static A.B.C.D Serde.IDeserialize<A.B.C.D>.Deserialize(IDeserializer deserializer)
                {
                    int _l_field = default !;
                    byte _r_assignedValid = 0;
                    var _l_typeInfo = DSerdeTypeInfo.TypeInfo;
                    var typeDeserialize = deserializer.DeserializeType(_l_typeInfo);
                    int _l_index_;
                    while ((_l_index_ = typeDeserialize.TryReadIndex(_l_typeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
                    {
                        switch (_l_index_)
                        {
                            case 0:
                                _l_field = typeDeserialize.ReadValue<int, Int32Wrap>(_l_index_);
                                _r_assignedValid |= ((byte)1) << 0;
                                break;
                            case Serde.IDeserializeType.IndexNotFound:
                                break;
                            default:
                                throw new InvalidOperationException("Unexpected index: " + _l_index_);
                        }
                    }

                    if ((_r_assignedValid & 0b1) != 0b1)
                    {
                        throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                    }

                    var newType = new A.B.C.D()
                    {
                        Field = _l_field,
                    };
                    return newType;
                }
            }
        }
    }
}