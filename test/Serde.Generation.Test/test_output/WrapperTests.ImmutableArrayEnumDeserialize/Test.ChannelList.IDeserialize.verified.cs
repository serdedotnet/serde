//HintName: Test.ChannelList.IDeserialize.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial record struct ChannelList : Serde.IDeserialize<Test.ChannelList>
    {
        static Test.ChannelList Serde.IDeserialize<Test.ChannelList>.Deserialize(IDeserializer deserializer)
        {
            System.Collections.Immutable.ImmutableArray<Test.Channel> _l_channels = default !;
            byte _r_assignedValid = 0b0;
            var _l_typeInfo = ChannelListSerdeTypeInfo.TypeInfo;
            var typeDeserialize = deserializer.DeserializeType(_l_typeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_typeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_channels = typeDeserialize.ReadValue<System.Collections.Immutable.ImmutableArray<Test.Channel>, ImmutableArrayWrap.DeserializeImpl<Test.Channel, Test.ChannelWrap>>(_l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case Serde.IDeserializeType.IndexNotFound:
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }

            if (_r_assignedValid != 0b1)
            {
                throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
            }

            var newType = new Test.ChannelList()
            {
                Channels = _l_channels,
            };
            return newType;
        }
    }
}