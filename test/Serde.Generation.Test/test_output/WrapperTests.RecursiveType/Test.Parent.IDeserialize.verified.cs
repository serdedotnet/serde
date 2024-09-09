//HintName: Test.Parent.IDeserialize.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial record Parent : Serde.IDeserialize<Test.Parent>
    {
        static Test.Parent Serde.IDeserialize<Test.Parent>.Deserialize(IDeserializer deserializer)
        {
            Recursive _l_r = default !;
            byte _r_assignedValid = 0;
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Parent>();
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_r = typeDeserialize.ReadValue<Recursive, Test.RecursiveWrap>(_l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case Serde.IDeserializeType.IndexNotFound:
                        typeDeserialize.SkipValue();
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }

            if ((_r_assignedValid & 0b1) != 0b1)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }

            var newType = new Test.Parent()
            {
                R = _l_r,
            };
            return newType;
        }
    }
}