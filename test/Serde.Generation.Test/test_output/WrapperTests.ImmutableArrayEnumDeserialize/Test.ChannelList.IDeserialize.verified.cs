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
            byte _r_assignedValid = 0;
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ChannelList>();
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_channels = typeDeserialize.ReadValue<System.Collections.Immutable.ImmutableArray<Test.Channel>, Serde.ImmutableArrayWrap.DeserializeImpl<Test.Channel, Test.ChannelWrap>>(_l_index_);
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

            var newType = new Test.ChannelList()
            {
                Channels = _l_channels,
            };
            return newType;
        }
    }
}