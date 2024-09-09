
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class GenericWrapperTests
    {
        partial record struct CustomArrayWrapExplicitOnType : Serde.IDeserialize<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>
        {
            static Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType Serde.IDeserialize<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>.Deserialize(IDeserializer deserializer)
            {
                Serde.Test.GenericWrapperTests.CustomImArray2<int> _l_a = default !;
                byte _r_assignedValid = 0;
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<CustomArrayWrapExplicitOnType>();
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_a = typeDeserialize.ReadValue<Serde.Test.GenericWrapperTests.CustomImArray2<int>, Serde.Test.GenericWrapperTests.CustomImArray2Wrap.DeserializeImpl<int, global::Serde.Int32Wrap>>(_l_index_);
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

                var newType = new Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType()
                {
                    A = _l_a,
                };
                return newType;
            }
        }
    }
}