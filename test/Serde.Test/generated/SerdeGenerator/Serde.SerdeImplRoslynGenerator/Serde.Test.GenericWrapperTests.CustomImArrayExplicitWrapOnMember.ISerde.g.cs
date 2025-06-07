
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class GenericWrapperTests
{
    partial record struct CustomImArrayExplicitWrapOnMember
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>.Serialize(Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteBoxedValue<Serde.Test.GenericWrapperTests.CustomImArray<int>, Serde.Test.GenericWrapperTests.CustomImArrayProxy.Ser<int, global::Serde.I32Proxy>>(_l_info, 0, value.A);
                _l_type.End(_l_info);
            }
            Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember Serde.IDeserialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>.Deserialize(IDeserializer deserializer)
            {
                Serde.Test.GenericWrapperTests.CustomImArray<int> _l_a = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_a = typeDeserialize.ReadBoxedValue<Serde.Test.GenericWrapperTests.CustomImArray<int>, Serde.Test.GenericWrapperTests.CustomImArrayProxy.De<int, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
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
                var newType = new Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember() {
                    A = _l_a,
                };

                return newType;
            }
        }
    }
}
