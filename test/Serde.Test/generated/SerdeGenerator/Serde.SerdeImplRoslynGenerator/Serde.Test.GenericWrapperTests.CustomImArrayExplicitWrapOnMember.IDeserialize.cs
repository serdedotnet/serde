
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class GenericWrapperTests
    {
        partial record struct CustomImArrayExplicitWrapOnMember : Serde.IDeserialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>
        {
            static Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember Serde.IDeserialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>.Deserialize(IDeserializer deserializer)
            {
                Serde.Test.GenericWrapperTests.CustomImArray<int> _l_a = default !;
                byte _r_assignedValid = 0;
                var _l_serdeInfo = CustomImArrayExplicitWrapOnMemberSerdeInfo.Instance;
                var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_a = typeDeserialize.ReadValue<Serde.Test.GenericWrapperTests.CustomImArray<int>, Serde.Test.GenericWrapperTests.CustomImArrayWrap.DeserializeImpl<int, Int32Wrap>>(_l_index_);
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

                var newType = new Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember()
                {
                    A = _l_a,
                };
                return newType;
            }
        }
    }
}